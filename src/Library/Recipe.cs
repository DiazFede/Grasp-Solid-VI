using System;
using System.Threading;
using System.Collections.Generic;

namespace Full_GRASP_And_SOLID
{
public class Recipe : IRecipeContent
{
    private IList<BaseStep> steps = new List<BaseStep>();
    public bool cook { get; private set; } = false;

    private CountdownTimer countdownTimer;
    public Product FinalProduct { get; set; }

    public void AddStep(Product input, double quantity, Equipment equipment, int time)
    {
        Step step = new Step(input, quantity, equipment, time);
        this.steps.Add(step);
    }

    public void AddStep(string description, int time)
    {
        WaitStep step = new WaitStep(description, time);
        this.steps.Add(step);
    }

    public void RemoveStep(BaseStep step)
    {
        this.steps.Remove(step);
    }

    public string GetTextToPrint()
    {
        string result = $"Receta de {this.FinalProduct.Description}:\n";
        foreach (BaseStep step in this.steps)
        {
            result = result + step.GetTextToPrint() + "\n";
        }

        result = result + $"Costo de producción: {this.GetProductionCost()}";

        return result;
    }

    public double GetProductionCost()
    {
        double result = 0;

        foreach (BaseStep step in this.steps)
        {
            result = result + step.GetStepCost();
        }

        return result;
    }

    public int GetCookTime()
    {
        int TotalCookTime = 0;
        foreach (BaseStep step in this.steps)
        {
            TotalCookTime += step.Time;
        }
        return TotalCookTime;
    }

    public void Cook()
    {
        int totalTime = GetCookTime();

        RecipeTimerAdapter timerAdapter = new(this);

        countdownTimer = new CountdownTimer();
        countdownTimer.Register(totalTime, timerAdapter);
    }

    public void TimeOut()
    {
        this.cook = true;
        Console.WriteLine("Listo");
    }
    public class RecipeTimerAdapter : TimerClient
    {
        private Recipe recipe;

        public RecipeTimerAdapter(Recipe recipe)
        {
            this.recipe = recipe;
        }

        public void TimeOut()
        {
            recipe.TimeOut();
        }
    }
}
}
