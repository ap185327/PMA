// <copyright file="SolutionTreeNodeViewModel.cs" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

using Microsoft.Extensions.Logging;
using Microsoft.Toolkit.Mvvm.Messaging;
using PMA.Application.Extensions;
using PMA.Application.ViewModels.Controls.Base;
using PMA.Domain.Enums;
using PMA.Domain.Interfaces.Interactors.Primary;
using PMA.Domain.Interfaces.Locators;
using PMA.Domain.Interfaces.ViewModels.Controls;
using PMA.Domain.Messages;
using PMA.Domain.Models;
using System.Linq;
using System.Text;
using System.Threading;
using PMA.Domain.InputPorts;

namespace PMA.Application.ViewModels.Controls
{
    /// <summary>
    /// The solution tree node view model class.
    /// </summary>
    public sealed class SolutionTreeNodeViewModel : TreeNodeViewModelBase<SolutionTreeNodeViewModel>, ISolutionTreeNodeViewModel
    {
        /// <summary>
        /// The morphological solution.
        /// </summary>
        private readonly Solution _solution;

        /// <summary>
        /// Initializes the new instance of <see cref="SolutionTreeNodeViewModel"/> class.
        /// </summary>
        /// <param name="parent">The parent tree node view model.</param>
        /// <param name="solution">The morphological solution.</param>
        /// <param name="interactor">The tree node view model interactor.</param>
        /// <param name="serviceLocator">The service locator.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="messenger">The messenger.</param>
        public SolutionTreeNodeViewModel(IWordFormTreeNodeViewModel parent,
            Solution solution,
            ITreeNodeInteractor interactor,
            IServiceLocator serviceLocator,
            ILogger<SolutionTreeNodeViewModel> logger,
            IMessenger messenger) : base(parent,
            interactor,
            serviceLocator,
            logger,
            messenger)
        {
            _solution = solution;
        }

        #region Overrides of ObservableRecipient

        /// <summary>
        /// Raised whenever the <see cref="P:Microsoft.Toolkit.Mvvm.ComponentModel.ObservableRecipient.IsActive" /> property is set to <see langword="true" />.
        /// Use this method to register to messages and do other initialization for this instance.
        /// </summary>
        /// <remarks>
        /// The base implementation registers all messages for this recipients that have been declared
        /// explicitly through the <see cref="T:Microsoft.Toolkit.Mvvm.Messaging.IRecipient`1" /> interface, using the default channel.
        /// For more details on how this works, see the <see cref="M:Microsoft.Toolkit.Mvvm.Messaging.IMessengerExtensions.RegisterAll(Microsoft.Toolkit.Mvvm.Messaging.IMessenger,System.Object)" /> method.
        /// If you need more fine tuned control, want to register messages individually or just prefer
        /// the lambda-style syntax for message registration, override this method and register manually.
        /// </remarks>
        protected override async void OnActivated()
        {
            base.OnActivated();

            _cancellationTokenSource = new CancellationTokenSource();

            if (_solution.Left is not null)
            {
                var inputPort = new GetWordFormTreeNodeInputPort
                {
                    Parent = this,
                    WordForm = _solution.Left
                };

                var result = await Interactor.GetWordFormTreeNodeAsync(inputPort, _cancellationTokenSource.Token);

                if (!IsOperationResultSuccess(result)) return;

                LeftNode = result.Result;
            }

            if (_solution.Right is not null)
            {
                var inputPort = new GetWordFormTreeNodeInputPort
                {
                    Parent = this,
                    WordForm = _solution.Right
                };

                var result = await Interactor.GetWordFormTreeNodeAsync(inputPort, _cancellationTokenSource.Token);

                if (!IsOperationResultSuccess(result)) return;

                RightNode = result.Result;
            }
        }

        /// <summary>
        /// Raised whenever the <see cref="P:Microsoft.Toolkit.Mvvm.ComponentModel.ObservableRecipient.IsActive" /> property is set to <see langword="false" />.
        /// Use this method to unregister from messages and do general cleanup for this instance.
        /// </summary>
        /// <remarks>
        /// The base implementation unregisters all messages for this recipient. It does so by
        /// invoking <see cref="M:Microsoft.Toolkit.Mvvm.Messaging.IMessenger.UnregisterAll(System.Object)" />, which removes all registered
        /// handlers for a given subscriber, regardless of what token was used to register them.
        /// That is, all registered handlers across all subscription channels will be removed.
        /// </remarks>
        protected override void OnDeactivated()
        {
            base.OnDeactivated();

            if (!_cancellationTokenSource.IsCancellationRequested)
            {
                _cancellationTokenSource.Cancel();
            }

            _cancellationTokenSource = null;
        }

        #endregion

        #region Overrides of TreeNodeViewModelBase<SolutionTreeNodeViewModel>

        /// <summary>
        /// Gets or sets a node text.
        /// </summary>
        public override string Text => GetNodeText();

        /// <summary>
        /// Gets a node tooltip text.
        /// </summary>
        public override string ToolTipText => GetToolTipText();

        /// <summary>
        /// Gets a sandhi text.
        /// </summary>
        public override string SandhiText => GetSandhiText();

        /// <summary>
        /// Gets a sandhi line.
        /// </summary>
        public override string SandhiLine => GetSandhiLine();

        /// <summary>
        /// Gets a tag object.
        /// </summary>
        public override object Tag => _solution;

        /// <summary>
        /// Gets whether tree node has child nodes.
        /// </summary>
        public override bool HasChildren => LeftNode is not null;

        /// <summary>
        /// Selects the node.
        /// </summary>
        protected override void Select()
        {
            var morphEntryMessage = new MorphEntryMessage
            {
                MorphEntry = _solution.GetMorphEntry(((WordForm)Parent.Tag).Entry)
            };

            Messenger.Send(morphEntryMessage);

            var morphRuleMessage = new MorphRuleMessage
            {
                MorphRules = _solution.Rules,
                SandhiMatches = _solution.Sandhi
            };

            Messenger.Send(morphRuleMessage);
        }

        #endregion

        #region Implementation of ISolutionTreeNodeViewModel

        /// <summary>
        /// Gets whether the solution is taken from dictionary or not.
        /// </summary>
        public bool IsFromDict => _solution.Content.Id > 0;

        /// <summary>
        /// Gets the solution relative rating: 0 - minimum, 1 - maximum.
        /// </summary>
        public double RelativeRating
        {
            get
            {
                double rating = _solution.Rating;

                if (rating == 0)
                {
                    return 0;
                }

                double maxRating = ((WordForm)Parent.Tag).GetMaxRating();
                double minRating = ((WordForm)Parent.Tag).GetMinRating();

                // ReSharper disable once CompareOfFloatsByEqualityOperator
                return maxRating == minRating ? 1 : (rating - minRating) / (maxRating - minRating);
            }
        }

        /// <summary>
        /// Gets morphological parameters.
        /// </summary>
        public byte[] Parameters => _solution.Content.Parameters;

        /// <summary>
        /// Gets whether the solution is virtual (doesn't exist in the live language) or not.
        /// </summary>
        public bool IsVirtual => _solution.Content.IsVirtual;

        /// <summary>
        /// Gets a solution error.
        /// </summary>
        public SolutionError Error => _solution.Content.Error;

        /// <summary>
        /// Gets or sets the left wordform node view model in the solution.
        /// </summary>
        public IWordFormTreeNodeViewModel LeftNode { get; private set; }

        /// <summary>
        /// Gets or sets the right wordform node view model in the solution.
        /// </summary>
        public IWordFormTreeNodeViewModel RightNode { get; private set; }

        #endregion

        #region Private Fields

        /// <summary>
        /// The cancellation token source.
        /// </summary>
        private CancellationTokenSource _cancellationTokenSource;

        #endregion

        #region Private Methods

        /// <summary>
        /// Gets a node text from the solution.
        /// </summary>
        /// <returns>The node text.</returns>
        private string GetNodeText()
        {
            var stringBuilder = new StringBuilder();

            var inputPort = new ExtractMorphInfoInputPort
            {
                UseVisibility = true,
                Parameters = _solution.Content.Parameters
            };

            var result = Interactor.ExtractMorphInfoFromMorphParametersAsync(inputPort, _cancellationTokenSource.Token).Result;

            if (!IsOperationResultSuccess(result)) return string.Empty;

            stringBuilder.Append(result.Result);

            if (_solution.Content.Base != MorphBase.None)
            {
                if (LeftNode is null)
                {
                    stringBuilder.Append(" [?");
                }
                else
                {
                    stringBuilder.Append(" [" + LeftNode.SandhiText);
                }

                if (RightNode is null)
                {
                    if (LeftNode is null || _solution.Content.Base is MorphBase.Unknown or MorphBase.Right or MorphBase.Both)
                    {
                        stringBuilder.Append(" + ?]");
                    }
                    else
                    {
                        stringBuilder.Append("]");
                    }
                }
                else
                {
                    stringBuilder.Append(" + " + RightNode.SandhiText + "]");
                }
            }

            if (!ServiceLocator.SettingService.GetValue<bool>("Options.DebugMode")) return stringBuilder.ToString();

            stringBuilder.Append(" => ");

            if (_solution.Rules != null)
            {
                stringBuilder.Append("Rule ID: " + string.Join(",", _solution.Rules.Select(x => x.Id)) + "; ");
            }

            stringBuilder.Append("Rating: " + _solution.Rating + "; Max Depth: " + _solution.GetMaxDepthLevel());

            return stringBuilder.ToString();
        }

        /// <summary>
        /// Gets a tooltip text from the solution.
        /// </summary>
        /// <returns>The tooltip text.</returns>
        private string GetToolTipText()
        {
            string toolTipText = ServiceLocator.SettingService.GetValue<bool>("Options.DebugMode")
                ? ServiceLocator.TranslateService.Translate(_solution.Content.Error)
                : _solution.Rules is null
                    ? null
                    : string.Join("\n", _solution.Rules.Select(x => x.Description).Distinct());

            return toolTipText;
        }

        /// <summary>
        /// Gets a text for sandhi.
        /// </summary>
        /// <returns>The sandhi text.</returns>
        private string GetSandhiText()
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.Append(Parent.SandhiLine);

            if (LeftNode is null && RightNode is null)
            {
                return stringBuilder.ToString();
            }

            if (LeftNode is null)
            {
                stringBuilder.Append(" = ?");
            }
            else
            {
                stringBuilder.Append(" = " + LeftNode.Text);
            }

            if (RightNode is null)
            {
                if (_solution.Content.Base is MorphBase.Unknown or MorphBase.Right or MorphBase.Both)
                {
                    stringBuilder.Append(" + ?");
                }
            }
            else
            {
                stringBuilder.Append(" + " + RightNode.Text);
            }

            return stringBuilder.ToString();
        }

        /// <summary>
        /// Gets a sandhi line from the solution tree node view model.
        /// </summary>
        /// <returns>A sandhi line.</returns>
        private string GetSandhiLine()
        {
            return GetSandhiText();
        }

        #endregion
    }
}
