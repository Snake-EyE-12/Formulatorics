using System.Collections.Generic;
using System.Linq;
using Cobra.DesignPattern;

public class CardInteractionManager : Singleton<ICardInteractionManager>, ICardInteractionManager
{
    private List<ICardSelectionInteractionRule> selectionRules = new List<ICardSelectionInteractionRule>();
    private ICardSelectionRuleData data;
    public void AddSelectionRule(ICardSelectionInteractionRule rule)
    {
        selectionRules.Add(rule);
    }

    public void TrySelect(ICardSelectable selectable)
    {
        if (selectionRules.All((x) => x.ConditionMet(selectable, data)))
        {
            selectable.Select();
            return;
        }
        selectable.FailSelectAttempt();
    }
}

public class SelectionData : ICardSelectionRuleData
{
    public int SelectedCount { get; set; }
}

public interface ICardInteractionManager
{
    public void AddSelectionRule(ICardSelectionInteractionRule rule);
    public void TrySelect(ICardSelectable selectable);
}

public interface ICardSelectionInteractionRule
{
    public bool ConditionMet(ICardSelectable candidate, ICardSelectionRuleData referenceData);
}

public interface ICardSelectable
{
    public void Select();
    public void Deselect();
    public void FailSelectAttempt();
}

public interface ICardSelectionRuleData
{
    public int SelectedCount { get; set; }
}

public class MaxSelectionRule : ICardSelectionInteractionRule
{
    private int maxSelection = 5;
    public bool ConditionMet(ICardSelectable candidate, ICardSelectionRuleData referenceData)
    {
        return referenceData.SelectedCount < maxSelection;
    }
}