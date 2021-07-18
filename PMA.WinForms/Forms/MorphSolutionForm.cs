// <copyright file="MorphSolutionForm.cs" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

using Autofac;
using PMA.Domain.Interfaces.Services;
using PMA.Domain.Interfaces.ViewModels;
using PMA.Domain.Interfaces.ViewModels.Controls;
using PMA.Domain.Interfaces.ViewModels.Controls.Base;
using PMA.WinForms.Extensions;
using PMA.WinForms.Helpers;
using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace PMA.WinForms.Forms
{
    public partial class MorphSolutionForm : Form
    {
        /// <summary>
        /// The maximum tree depth level.
        /// </summary>
        private const int MaxTreeViewDepthLevel = 2;

        /// <summary>
        /// The morphological solution view model.
        /// </summary>
        private readonly IMorphSolutionViewModel _morphSolutionViewModel;

        /// <summary>
        /// The translate service.
        /// </summary>
        private readonly ITranslateService _translateService;

        /// <summary>
        /// Initializes a new instance of the MorphSolutionForm class.
        /// </summary>
        public MorphSolutionForm()
        {
            _morphSolutionViewModel = Program.Scope.Resolve<IMorphSolutionViewModel>();
            _translateService = Program.Scope.Resolve<ITranslateService>();

            InitializeComponent();
            OverrideStrings();
            SetDefaultValues();
            SubscribeEvents();
        }

        #region Initialization methods

        /// <summary>
        /// Overrides control strings.
        /// </summary>
        private void OverrideStrings()
        {
            Text = _translateService.Translate(Name);
        }

        /// <summary>
        /// Sets default values for control and fields.
        /// </summary>
        private void SetDefaultValues()
        {
            SandhiLabel.Text = string.Empty;
        }

        /// <summary>
        /// Subscribes events.
        /// </summary>
        private void SubscribeEvents()
        {
            _morphSolutionViewModel.PropertyChanged += ViewModel_PropertyChanged;
        }

        #endregion Initialization methods

        /// <summary>
        /// Event handler for the view model property changed.
        /// </summary>
        /// <param name="sender">Object sender.</param>
        /// <param name="e">Event arguments.</param>
        private void ViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            this.InvokeIfNeeded(() =>
            {
                switch (e.PropertyName)
                {
                    case "MainTreeNode":
                        {
                            if (_morphSolutionViewModel.MainTreeNode is null)
                            {
                                SandhiLabel.Text = string.Empty;
                                SolutionTreeView.Nodes.Clear();
                            }
                            else
                            {
                                var mainTreeNode = TreeNodeHelper.GetTreeNode(_morphSolutionViewModel.MainTreeNode);

                                AddChildNodes(mainTreeNode, MaxTreeViewDepthLevel);

                                SolutionTreeView.Nodes.Add(mainTreeNode);
                            }
                            break;
                        }
                }
            });
        }

        /// <summary>
        /// Event handler for the MorphSolutionForm location changed.
        /// </summary>
        /// <param name="sender">Object sender.</param>
        /// <param name="e">Event arguments.</param>
        private void MorphSolutionForm_LocationChanged(object sender, EventArgs e)
        {
            this.Sticking();
        }

        /// <summary>
        /// Event handler for the SolutionTreeView before expand.
        /// </summary>
        /// <param name="sender">Object sender.</param>
        /// <param name="e">Event arguments.</param>
        private void SolutionTreeView_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            if (e.Node.Tag is IWordFormTreeNodeViewModel)
            {
                AddChildNodes(e.Node, MaxTreeViewDepthLevel);
            }
            else
            {
                AddChildNodes(e.Node.Parent, MaxTreeViewDepthLevel + 2);
            }
        }

        /// <summary>
        /// Event handler for the SolutionTreeView after select.
        /// </summary>
        /// <param name="sender">Object sender.</param>
        /// <param name="e">Event arguments.</param>
        private void SolutionTreeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            SandhiLabel.Text = (SolutionTreeView.SelectedNode?.Tag as ITreeNodeViewModel)?.SandhiLine;
        }

        /// <summary>
        /// Event handler for the SolutionTreeView mouse double click.
        /// </summary>
        /// <param name="sender">Object sender.</param>
        /// <param name="e">Event arguments.</param>
        private void SolutionTreeView_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            (SolutionTreeView.SelectedNode?.Tag as ITreeNodeViewModel)?.SelectCommand.Execute(null);
        }

        /// <summary>
        /// Adds child nodes to the wordform node.
        /// </summary>
        /// <param name="wordFormNode">The wordform node.</param>
        /// <param name="currentDepthLevel">The current depth level of nodes.</param>
        private void AddChildNodes(TreeNode wordFormNode, int currentDepthLevel)
        {
            currentDepthLevel--;

            bool hasChildren = wordFormNode.Nodes.Count > 0;

            var wordFormTreeNodeViewModel = (IWordFormTreeNodeViewModel)wordFormNode.Tag;

            for (int i = 0; i < wordFormTreeNodeViewModel.SolutionNodes.Count; i++)
            {
                var solutionTreeNodeViewModel = wordFormTreeNodeViewModel.SolutionNodes[i];

                TreeNode solutionNode;
                if (hasChildren)
                {
                    solutionNode = wordFormNode.Nodes[i];
                }
                else
                {
                    solutionNode = TreeNodeHelper.GetTreeNode(solutionTreeNodeViewModel, SolutionTreeView.Font);

                    wordFormNode.Nodes.Add(solutionNode);
                }

                // LeftEntry
                if (solutionTreeNodeViewModel.LeftNode != null)
                {
                    if (solutionNode.Nodes.Count == 0)
                    {
                        solutionNode.Nodes.Add(TreeNodeHelper.GetTreeNode(solutionTreeNodeViewModel.LeftNode));
                    }

                    if (currentDepthLevel > MaxTreeViewDepthLevel)
                    {
                        AddChildNodes(solutionNode.Nodes[0], currentDepthLevel);
                    }
                }

                // RightEntry
                if (solutionTreeNodeViewModel.RightNode is null) continue;

                if (solutionNode.Nodes.Count == 1)
                {
                    solutionNode.Nodes.Add(TreeNodeHelper.GetTreeNode(solutionTreeNodeViewModel.RightNode));
                }

                if (currentDepthLevel > MaxTreeViewDepthLevel)
                {
                    AddChildNodes(solutionNode.Nodes[1], currentDepthLevel);
                }
            }
        }

        /// <summary>
        /// Event handler for the MorphSolutionForm visible changed.
        /// </summary>
        /// <param name="sender">Object sender.</param>
        /// <param name="e">Event arguments.</param>
        private void MorphSolutionForm_VisibleChanged(object sender, EventArgs e)
        {
            if (Visible)
            {
                _morphSolutionViewModel.OnAppearing();
            }
            else
            {
                _morphSolutionViewModel.OnDisappearing();
            }
        }
    }
}
