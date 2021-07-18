// <copyright file="WordFormTreeNodeViewModel.cs" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

using MediatR;
using Microsoft.Extensions.Logging;
using PMA.Application.Extensions;
using PMA.Application.ViewModels.Controls.Base;
using PMA.Domain.Interfaces.Interactors.Primary;
using PMA.Domain.Interfaces.Locators;
using PMA.Domain.Interfaces.ViewModels.Controls;
using PMA.Domain.Interfaces.ViewModels.Controls.Base;
using PMA.Domain.Models;
using PMA.Domain.Notifications;
using PMA.Utils.Extensions;
using System.Collections.Generic;
using System.Linq;

namespace PMA.Application.ViewModels.Controls
{
    /// <summary>
    /// The wordform tree node view model class.
    /// </summary>
    public sealed class WordFormTreeNodeViewModel : TreeNodeViewModelBase<WordFormTreeNodeViewModel>, IWordFormTreeNodeViewModel
    {
        /// <summary>
        /// Initializes the new instance of <see cref="WordFormTreeNodeViewModel"/> class.
        /// </summary>
        /// <param name="parent">The parent tree node view model.</param>
        /// <param name="wordForm">The wordform.</param>
        /// <param name="interactor">The tree node view model interactor.</param>
        /// <param name="serviceLocator">The service locator.</param>
        /// <param name="mediator">The mediator.</param>
        /// <param name="solutionLogger">The solution logger.</param>
        /// <param name="logger">The logger.</param>
        public WordFormTreeNodeViewModel(ITreeNodeViewModel parent,
            WordForm wordForm,
            ITreeNodeInteractor interactor,
            IServiceLocator serviceLocator,
            IMediator mediator,
            ILogger<SolutionTreeNodeViewModel> solutionLogger,
            ILogger<WordFormTreeNodeViewModel> logger) : base(parent,
            wordForm,
            interactor,
            serviceLocator,
            mediator,
            logger)
        {
            foreach (var solution in wordForm.Solutions)
            {
                SolutionNodes.Add(new SolutionTreeNodeViewModel(this, solution, Interactor, ServiceLocator, Mediator, logger, solutionLogger));
            }
        }

        #region Overrides of TreeNodeViewModelBase<WordFormTreeNodeViewModel>

        /// <summary>
        /// Gets a node text.
        /// </summary>
        public override string Text => GetNodeText();

        /// <summary>
        /// Gets a node tooltip text.
        /// </summary>
        public override string ToolTipText => null;

        /// <summary>
        /// Gets a sandhi text.
        /// </summary>
        public override string SandhiText => GetSandhiText();

        /// <summary>
        /// Gets a sandhi line.
        /// </summary>
        public override string SandhiLine => GetSandhiLine();

        /// <summary>
        /// Selects the node.
        /// </summary>
        protected override void Select()
        {
            Mediator.Publish(new MorphEntryNotification
            {
                MorphEntry = ((WordForm)Tag).GetMorphEntry()
            });
        }

        #endregion

        #region Implementation of IWordFormTreeNodeViewModel

        /// <summary>
        /// Gets a collection of solution node view models.
        /// </summary>
        public IList<ISolutionTreeNodeViewModel> SolutionNodes { get; } = new List<ISolutionTreeNodeViewModel>();

        #endregion

        /// <summary>
        /// Gets the node text from the wordform.
        /// </summary>
        /// <returns>The node text.</returns>
        private string GetNodeText()
        {
            string nodeText = null;

            if (Tag is null)
            {
                nodeText = "?";
            }
            else if (Parent is null)
            {
                var result = Interactor.GetLayerForFirstNode();

                if (!result.Success)
                {
                    Logger.LogErrors(result.Messages);
                }
                else
                {
                    nodeText = ServiceLocator.TranslateService.Translate("WordFormTreeNodeViewModel.FirstTreeNode",
                        GetFullEntry(),
                        result.Result);
                }
            }
            else
            {
                nodeText = GetFullEntry();
            }

            return nodeText;
        }

        /// <summary>
        /// Gets a text for sandhi.
        /// </summary>
        /// <returns>The sandhi text.</returns>
        private string GetSandhiText()
        {
            string sandhiText = Tag is null
                ? "?"
                : Parent != null && ((Solution)Parent.Tag).Content.Id != 0 &&
                  !((WordForm)Tag).IsFromDict((Solution)Parent.Tag)
                    ? GetFullEntry() + "?"
                    : GetFullEntry();

            return sandhiText;
        }

        /// <summary>
        /// Gets a sandhi line from the wordform tree node view model.
        /// </summary>
        /// <returns>A sandhi line.</returns>
        private string GetSandhiLine()
        {
            return Parent is null ? GetFullEntry() : $"{Parent.SandhiLine}; {Text}";
        }

        /// <summary>
        /// Gets a wordform entry with specific symbol (root).
        /// </summary>
        /// <returns>The wordform entry with specific symbol (root).</returns>
        private string GetFullEntry()
        {
            var wordForm = (WordForm)Tag;

            var morphEntry = wordForm.GetMorphEntry();

            byte rootTermId = ServiceLocator.SettingService.GetValue<byte>("Options.RootTermId");

            string fullEntry = morphEntry is null ? wordForm.Entry :
                morphEntry.Parameters.Any(x => x == rootTermId) ? "√" + wordForm.Entry : wordForm.Entry;

            return fullEntry;
        }
    }
}
