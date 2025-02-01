using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace HowMuch
{
    public class MarginData
    {
        public string RecipeKey { get; set; }
        public string RecipeName { get; set; }
        public float? TotalPrice { get; set; }
        public float? Profit { get; set; }

        public MarginData()
        {

        }
        public MarginData(StockData data)
        {
            RecipeKey = data.RecipeKey;
            RecipeName = data.RecipeName;
            TotalPrice = data.TotalPrice;
            Profit = data.TotalPrice - (data.PricePerUnit * data.Quantity);
        }
    }
    public class StockData
    {
        public string RecipeKey { get; set; }
        public string RecipeName { get; set; }
        public float? Price { get; set; }
        public float? PricePerUnit { get; set; }
        public int Quantity { get; set; }
        public float? TotalPrice { get; set; }
        public float? Profit { get; set; }

        public StockData()
        {

        }
        public StockData(RecipeData data, int quantity)
        {
            RecipeKey = data.RecipeKey;
            RecipeName = data.RecipeName;
            Price = data.Price;
            PricePerUnit = data.PricePerUnit;
            Quantity = quantity;
            TotalPrice = Quantity * Price;
            Profit = TotalPrice - (data.PricePerUnit * quantity);
        }
    }
    public class RecipeData
    {
        public string RecipeKey { get; set; }
        public string RecipeName { get; set; }
        public float? Price { get; set; }
        public float? PricePerUnit { get; set; }
        public float? CostRatio { get; set; }

        public RecipeData()
        {

        }
        public RecipeData(RecipeParam param)
        {
            RecipeKey = param.RecipeKey;
            RecipeName = param.RecipeName;
            PricePerUnit = param.PricePerUnit;
            Price = param.Price;
            CostRatio = param.CostRatio;
        }
    }
    public class RecipeDetailData
    {
        public string RecipeKey { get; set; }
        public string RecipeName { get; set; }
        public int Seq { get; set; }
        public string RecipeDetailKey { get; set; }
        public string IngredientKey { get; set; }
        public string IngredientName { get; set; }
        public float? MaterialUsage { get; set; }
        public string UnitKey { get; set; }
        public string UnitName { get; set; }

        public RecipeDetailData()
        {

        }
        public RecipeDetailData(RecipeDetailParam param)
        {
            RecipeKey = param.RecipeKey;
            RecipeName = param.RecipeName;
            Seq = param.Seq;
            RecipeDetailKey = param.RecipeDetailKey;
            IngredientKey = param.IngredientKey;
            IngredientName = param.IngredientName;
            MaterialUsage = param.MaterialUsage;
            UnitKey = param.UnitKey;
            UnitName = param.UnitName;
        }
    }
    public class SourceData
    {
        public string SourceKey { get; set; }
        public string SourceName { get; set; }
        public float? Amount { get; set; }
        public float? PricePerUnit { get; set; }

        public SourceData() 
        { 

        }
        public SourceData(SourceParam param)
        {
            SourceKey = param.SourceKey;
            SourceName = param.SourceName;
            Amount = param.Amount;
            PricePerUnit = param.PricePerUnit;
        }
    }
    public class SourceRecipeData
    {
        public string SourceKey { get; set; }
        public string SourceName { get; set; }
        public int Seq { get; set; }
        public string SourceRecipeKey { get; set; }
        public string IngredientKey { get; set; }
        public string IngredientName { get; set; }
        public float? MaterialUsage { get; set; }
        public string UnitKey { get; set; }
        public string UnitName { get; set; }

        public SourceRecipeData()
        {

        }
        public SourceRecipeData(SourceRecipeParam param)
        {
            SourceKey = param.SourceKey;
            SourceName = param.SourceName;
            Seq = param.Seq;
            SourceRecipeKey = param.SourceRecipeKey;
            IngredientKey = param.IngredientKey;
            IngredientName = param.IngredientName;
            MaterialUsage = param.MaterialUsage;
            UnitKey = param.UnitKey;
            UnitName = param.UnitName;
        }
    }
    public class IngredientData
    {
        public string IngredientKey { get; set; }
        public string IngredientName { get; set; }
        public float? UnitValue { get; set; }
        public string UnitKey { get; set; }
        public string UnitName { get; set; }
        public float? Price { get; set; }
        public float? PricePerUnit { get; set; }

        public IngredientData(IngredientParam param)
        {
            IngredientKey = param.IngredientKey;
            IngredientName = param.IngredientName;
            UnitValue = param.UnitValue;
            UnitKey = param.UnitKey;
            UnitName = param.UnitName;
            Price = param.Price;
            PricePerUnit = param.PricePerUnit;
        }
    }
    public class UnitData
    {
        public string ParentUnitKey { get; set; }
        public string ParentUnitRelation { get; set; }
        public string UnitKey { get; set; }
        public string UnitName { get; set; }
        public float? Value { get; set; }
        public string Desc { get; set; }
        public ObservableCollection<UnitData> Childs { get; set; }

        public UnitData(UnitParam param)
        {
            ParentUnitKey = param.ParentUnitKey;
            ParentUnitRelation = param.ParentUnitRelation.ToString();
            UnitKey = param.UnitKey;
            UnitName = param.UnitName;
            Value = param.Value;
            Desc = param.Description;
            Childs = null;
        }
    }
}
