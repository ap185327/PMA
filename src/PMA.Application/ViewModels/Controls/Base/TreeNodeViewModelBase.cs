// <copyright file="TreeNodeViewModelBase.cs" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

using Microsoft.Extensions.Logging;
using Microsoft.Toolkit.Mvvm.Input;
using Microsoft.Toolkit.Mvvm.Messaging;
using PMA.Application.ViewModels.Base;
using PMA.Domain.Interfaces.Interactors.Primary;
using PMA.Domain.Interfaces.Locators;
using PMA.Domain.Interfaces.ViewModels.Controls.Base;
using System.Windows.Input;

namespace PMA.Application.ViewModels.Controls.Base
{
    /// <summary>
    /// The base tree node view model class.
    /// </summary>
    public abstract class TreeNodeViewModelBase<T> : ViewModelBase<T>, ITreeNodeViewModel where T : class
    {
        /// <summary>
        /// The tree node view model interactor.
        /// </summary>
        protected readonly ITreeNodeInteractor Interactor;

        /// <summary>
        /// Initializes the new instance of <see cref="TreeNodeViewModelBase{T}"/> class.
        /// </summary>
        /// <param name="parent">The parent tree node view model.</param>
        /// <param name="interactor">The tree node view model interactor.</param>
        /// <param name="serviceLocator">The service locator.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="messenger">The messenger.</param>
        protected TreeNodeViewModelBase(ITreeNodeViewModel parent, ITreeNodeInteractor interactor, IServiceLocator serviceLocator, ILogger<T> logger, IMessenger messenger) : base(serviceLocator, logger, messenger)
        {
            Parent = parent;
            Interactor = interactor;

            SelectCommand = new RelayCommand(Select);
        }

        #region Implementation of ITreeNodeViewModel

        /// <summary>
        /// Gets a node text.
        /// </summary>
        public abstract string Text { get; }

        /// <summary>
        /// Gets a node tooltip text.
        /// </summary>
        public abstract string ToolTipText { get; }

        /// <summary>
        /// Gets a sandhi text.
        /// </summary>
        public abstract string SandhiText { get; }

        /// <summary>
        /// Gets a sandhi line.
        /// </summary>
        public abstract string SandhiLine { get; }

        /// <summary>
        /// Gets a tag object.
        /// </summary>
        public abstract object Tag { get; }

        /// <summary>
        /// Gets whether tree node has child nodes.
        /// </summary>
        public abstract bool HasChildren { get; }

        /// <summary>
        /// Gets a parent tree node view model.
        /// </summary>
        public ITreeNodeViewModel Parent { get; }

        /// <summary>
        /// Gets a command to select node.
        /// </summary>
        public ICommand SelectCommand { get; }

        #endregion

        /// <summary>
        /// Selects the node.
        /// </summary>
        protected abstract void Select();
    }
}
