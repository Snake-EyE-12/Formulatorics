using System;
using System.Collections.Generic;
using System.Linq;
using Cobra.DesignPattern;
using UnityEngine;

public class CardGroupInteractionHandler : MonoBehaviour, IGroupInteractionControl
{
    private void Awake()
    {
        ServiceLocator.Register<IGroupInteractionControl>(this);
    }

    private bool gathering;


    private bool listeningForClicks;

    public void OnHoveredOverSomething()
    {
        listeningForClicks = false;
    }

    public void OnHoveringNothing()
    {
        listeningForClicks = true;
    }
    
    public void PressDown()
    {
        if (!listeningForClicks) return;
        gathering = true;
    }
    public void ReleaseDown()
    {
        gathering = false;
        hovered.ForEach((x) => x.PerformSelect());
        hovered.Clear();
    }

    
    private List<IQuickSelectable> hovered = new List<IQuickSelectable>();
    public void Add_Hover(IQuickSelectable selectable)
    {
        if(!gathering) return;
        hovered.Add(selectable);
    }
    
    
    
    #region Deselect
    private List<IQuickSelectable> selected = new List<IQuickSelectable>();
    public void Add_Select(IQuickSelectable selectable)
    {
        selected.Add(selectable);
    }
    public void PerformQuickDeselect()
    {
        selected.ForEach((x) => x.PerformDeselect());
        selected.Clear();
    }

    #endregion
    
}

public interface IGroupInteractionControl : IService
{
    public void PressDown();
    public void ReleaseDown();
    public void Add_Hover(IQuickSelectable selectable);
    public void OnHoveredOverSomething();
    public void OnHoveringNothing();
    
    
    
    
    
    public void Add_Select(IQuickSelectable selectable);
    public void PerformQuickDeselect();
}


/*
 * private List<ICardSelectionInteractionRule> selectionRules = new List<ICardSelectionInteractionRule>();
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
 */

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