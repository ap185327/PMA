// <copyright file="TreeNodeViewModelBase.cs" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Toolkit.Mvvm.Input;
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
        /// <param name="tag">The tag object.</param>
        /// <param name="interactor">The tree node view model interactor.</param>
        /// <param name="serviceLocator">The service locator.</param>
        /// <param name="mediator">The mediator.</param>
        /// <param name="logger">The logger.</param>
        protected TreeNodeViewModelBase(ITreeNodeViewModel parent, object tag, ITreeNodeInteractor interactor, IServiceLocator serviceLocator, IMediator mediator, ILogger<T> logger) : base(serviceLocator, mediator, logger)
        {
            Parent = parent;
            Tag = tag;
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
        public object Tag { get; }

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
