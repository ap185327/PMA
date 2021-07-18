// <copyright file="SolutionTreeNodeViewModel.cs" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

using MediatR;
using Microsoft.Extensions.Logging;
using PMA.Application.Extensions;
using PMA.Application.ViewModels.Controls.Base;
using PMA.Domain.Enums;
using PMA.Domain.Interfaces.Interactors.Primary;
using PMA.Domain.Interfaces.Locators;
using PMA.Domain.Interfaces.ViewModels.Controls;
using PMA.Domain.Interfaces.ViewModels.Controls.Base;
using PMA.Domain.Models;
using PMA.Domain.Notifications;
using PMA.Utils.Extensions;
using System.Linq;
using System.Text;

namespace PMA.Application.ViewModels.Controls
{
    /// <summary>
    /// The solution tree node view model class.
    /// </summary>
    public sealed class SolutionTreeNodeViewModel : TreeNodeViewModelBase<SolutionTreeNodeViewModel>, ISolutionTreeNodeViewModel
    {
        /// <summary>
        /// Initializes the new instance of <see cref="SolutionTreeNodeViewModel"/> class.
        /// </summary>
        /// <param name="parent">The parent tree node view model.</param>
        /// <param name="solution">>The morphological solution.</param>
        /// <param name="interactor">The tree node view model interactor.</param>
        /// <param name="serviceLocator">The service locator.</param>
        /// <param name="mediator">The mediator.</param>
        /// <param name="wordFormLogger">The wordform logger.</param>
        /// <param name="logger">The logger.</param>
        public SolutionTreeNodeViewModel(ITreeNodeViewModel parent,
            Solution solution,
            ITreeNodeInteractor interactor,
            IServiceLocator serviceLocator,
            IMediator mediator,
            ILogger<WordFormTreeNodeViewModel> wordFormLogger,
            ILogger<SolutionTreeNodeViewModel> logger) : base(parent,
            solution,
            interactor,
            serviceLocator,
            mediator,
            logger)
        {
            if (solution.Left is not null)
            {
                LeftNode = new WordFormTreeNodeViewModel(this, solution.Left, interactor, ServiceLocator, Mediator, logger, wordFormLogger);
            }

            if (solution.Right is not null)
            {
                RightNode = new WordFormTreeNodeViewModel(this, solution.Right, interactor, ServiceLocator, Mediator, logger, wordFormLogger);
            }
        }

        #region Overrides of TreeNodeViewModelBase<SolutionTreeNodeViewModel>

        /// <summary>
        /// Gets a node text.
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
        /// Selects the node.
        /// </summary>
        protected override void Select()
        {
            var solution = (Solution)Tag;

            Mediator.Publish(new MorphEntryNotification
            {
                MorphEntry = solution.GetMorphEntry(((WordForm)Parent.Tag).Entry)
            });

            Mediator.Publish(new MorphRuleNotification
            {
                MorphRules = solution.Rules,
                SandhiMatches = solution.Sandhi
            });
        }

        #endregion

        #region Implementation of ISolutionTreeNodeViewModel

        /// <summary>
        /// Gets whether the solution is taken from dictionary or not.
        /// </summary>
        public bool IsFromDict => ((Solution)Tag).Content.Id > 0;

        /// <summary>
        /// Gets the solution relative rating: 0 - minimum, 1 - maximum.
        /// </summary>
        public double RelativeRating
        {
            get
            {
                double rating = ((Solution)Tag).Rating;

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
        /// Gets whether the solution is virtual (doesn't exist in the live language) or not.
        /// </summary>
        public bool IsVirtual => ((Solution)Tag).Content.IsVirtual;

        /// <summary>
        /// Gets a solution error.
        /// </summary>
        public SolutionError Error => ((Solution)Tag).Content.Error;

        /// <summary>
        /// Gets the left wordform node view model in the solution.
        /// </summary>
        public IWordFormTreeNodeViewModel LeftNode { get; }

        /// <summary>
        /// Gets the right wordform node view model in the solution.
        /// </summary>
        public IWordFormTreeNodeViewModel RightNode { get; }

        #endregion

        /// <summary>
        /// Gets a node text from the solution.
        /// </summary>
        /// <returns>The node text.</returns>
        private string GetNodeText()
        {
            var solution = (Solution)Tag;

            var stringBuilder = new StringBuilder();

            var result = Interactor.ExtractMorphInfoFromMorphParameters(solution.Content.Parameters);

            if (!result.Success)
            {
                Logger.LogErrors(result.Messages);
            }
            else
            {
                stringBuilder.Append(result.Result);

                if (solution.Content.Base != MorphBase.None)
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
                        if (LeftNode is null || solution.Content.Base is MorphBase.Unknown or MorphBase.Right or MorphBase.Both)
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

                if (solution.Rules != null)
                {
                    stringBuilder.Append("Rule ID: " + string.Join(",", solution.Rules.Select(x => x.Id)) + "; ");
                }

                stringBuilder.Append("Rating: " + solution.Rating + "; Max Depth: " + solution.GetMaxDepthLevel());
            }

            return stringBuilder.ToString();
        }

        /// <summary>
        /// Gets a tooltip text from the solution.
        /// </summary>
        /// <returns>The tooltip text.</returns>
        private string GetToolTipText()
        {
            var solution = (Solution)Tag;

            string toolTipText = ServiceLocator.SettingService.GetValue<bool>("Options.DebugMode")
                ? ServiceLocator.TranslateService.Translate(solution.Content.Error)
                : solution.Rules is null
                    ? null
                    : string.Join("\n", solution.Rules.Select(x => x.Description).Distinct());

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

            if (LeftNode is not null || RightNode is not null)
            {
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
                    var solution = (Solution)Tag;

                    if (solution.Content.Base is MorphBase.Unknown or MorphBase.Right or MorphBase.Both)
                    {
                        stringBuilder.Append(" + ?");
                    }
                }
                else
                {
                    stringBuilder.Append(" + " + RightNode.Text);
                }
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
    }
}
