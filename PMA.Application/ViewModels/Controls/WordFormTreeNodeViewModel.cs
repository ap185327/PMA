// <copyright file="WordFormTreeNodeViewModel.cs" company="Andrey Pospelov">
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
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace PMA.Application.ViewModels.Controls
{
    /// <summary>
    /// The wordform tree node view model class.
    /// </summary>
    public sealed class WordFormTreeNodeViewModel : TreeNodeViewModelBase<WordFormTreeNodeViewModel>, IWordFormTreeNodeViewModel
    {
        /// <summary>
        /// The wordform.
        /// </summary>
        private readonly WordForm _wordForm;

        /// <summary>
        /// Initializes the new instance of <see cref="WordFormTreeNodeViewModel"/> class.
        /// </summary>
        /// <param name="parent">The parent tree node view model.</param>
        /// <param name="wordForm">The wordform.</param>
        /// <param name="interactor">The tree node view model interactor.</param>
        /// <param name="serviceLocator">The service locator.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="messenger">The messenger.</param>
        public WordFormTreeNodeViewModel(ISolutionTreeNodeViewModel parent,
            WordForm wordForm,
            ITreeNodeInteractor interactor,
            IServiceLocator serviceLocator,
            ILogger<WordFormTreeNodeViewModel> logger,
            IMessenger messenger) : base(parent,
            interactor,
            serviceLocator,
            logger,
            messenger)
        {
            _wordForm = wordForm;
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

            var groups = _wordForm.Solutions.Select(solution => new SolutionTreeNodeViewModelGroup
            {
                Parent = this,
                Solution = solution
            }).ToList();

            var result = await Interactor.GetSolutionTreeNodesAsync(groups, _cancellationTokenSource.Token);

            if (!IsOperationResultSuccess(result)) return;

            SolutionNodes = result.Result;
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
        /// Gets a tag object.
        /// </summary>
        public override object Tag => _wordForm;

        /// <summary>
        /// Gets whether tree node has child nodes.
        /// </summary>
        public override bool HasChildren => SolutionNodes.Count > 0;

        /// <summary>
        /// Selects the node.
        /// </summary>
        protected override void Select()
        {
            var message = new MorphEntryMessage
            {
                MorphEntry = _wordForm.GetMorphEntry()
            };

            Messenger.Send(message);
        }

        #endregion

        #region Implementation of IWordFormTreeNodeViewModel

        /// <summary>
        /// Gets a collection of solution node view models.
        /// </summary>
        public IList<ISolutionTreeNodeViewModel> SolutionNodes { get; private set; }

        #endregion

        #region Private Fields

        /// <summary>
        /// The cancellation token source.
        /// </summary>
        private CancellationTokenSource _cancellationTokenSource;

        #endregion

        #region Private Methods

        /// <summary>
        /// Gets the node text from the wordform.
        /// </summary>
        /// <returns>The node text.</returns>
        private string GetNodeText()
        {
            string nodeText;

            if (_wordForm is null)
            {
                nodeText = "?";
            }
            else if (Parent is null)
            {
                var result = Interactor.GetLayerForFirstNodeAsync(_cancellationTokenSource.Token).Result;

                if (!IsOperationResultSuccess(result)) return string.Empty;

                nodeText = ServiceLocator.TranslateService.Translate("WordFormTreeNodeViewModel.FirstTreeNode",
                    GetFullEntry(),
                    result.Result);
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
            string sandhiText = _wordForm is null
                ? "?"
                : Parent != null && ((Solution)Parent.Tag).Content.Id != 0 &&
                  !_wordForm.IsFromDict((Solution)Parent.Tag)
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
            var morphEntry = _wordForm.GetMorphEntry();

            byte rootTermId = ServiceLocator.SettingService.GetValue<byte>("Options.RootTermId");

            string fullEntry = morphEntry is null ? _wordForm.Entry :
                morphEntry.Parameters.Any(x => x == rootTermId) ? "√" + _wordForm.Entry : _wordForm.Entry;

            return fullEntry;
        }

        #endregion
    }
}
