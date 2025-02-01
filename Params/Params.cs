using System;
using System.Collections.Generic;
using System.Text;

namespace HowMuch
{
    public abstract class Param
    {
        public virtual string GetQuery() 
        {
            return string.Empty;
        }

        public virtual object GetParameter()
        {
            return this;
        }
    }
    public class UnitKeyNameParam : Param
    {
        public string UserId { get; set; }
        public string UnitKey { get; set; }
        public string UnitName { get; set; }

        public override string GetQuery()
        {
            return string.Format("?UnitKey={0}", UnitKey);
        }

        public override object GetParameter()
        {
            return this;
        }
    }
    public class UnitParam : Param
    {
        public string UserId;
        public string UnitKey;
        public string ParentUnitKey;
        public float? ParentUnitRelation;
        public string UnitName;
        public float? Value;
        public string Description;

        public override string GetQuery()
        {
            return string.Format("?Unitname={0}", UnitName);
        }

        public override object GetParameter()
        {
            return this;
        }
    }
    public class IngredientKeyParam : Param
    {
        public string UserId { get; set; }
        public string IngredientKey { get; set; }
        public string IngredientName { get; set; }
        public string UnitKey { get; set; }

        public IngredientKeyParam()
        {

        }
        public IngredientKeyParam(IngredientParam param)
        {
            IngredientKey = param.IngredientKey;
            IngredientName = param.IngredientName;
            UnitKey = param.UnitKey;
        }
        public override string GetQuery()
        {
            return string.Format("?UnitKey={0}", UnitKey);
        }

        public override object GetParameter()
        {
            return this;
        }
    }
    public class IngredientParam : Param
    {
        public string UserId;
        public string IngredientKey;
        public string IngredientName;
        public float? UnitValue;
        public string UnitKey;
        public string UnitName;
        public float? Price;
        public float? PricePerUnit;

        public override string GetQuery()
        {
            return string.Format("?IngredientKey={0}", IngredientKey);
        }

        public override object GetParameter()
        {
            return this;
        }
    }
    public class SourceParam : Param
    {
        public string UserId;
        public string SourceKey;
        public string SourceName;
        public float? Amount;
        public float? PricePerUnit;

        public override string GetQuery()
        {
            return string.Format("?SourceKey={0}", SourceKey);
        }

        public override object GetParameter()
        {
            return this;
        }
    }
    public class SourceRecipeParam : Param
    {
        public string UserId;
        public string SourceKey;
        public string SourceName;
        public string SourceRecipeKey;
        public int Seq;
        public string IngredientKey;
        public string IngredientName;
        public float? MaterialUsage;
        public string UnitKey;
        public string UnitName;

        public override string GetQuery()
        {
            return string.Format("?SourceKey={0}&SourceRecipeKey={1}", SourceKey, SourceRecipeKey);
        }

        public override object GetParameter()
        {
            return this;
        }
    }
    public class RecipeParam : Param
    {
        public string UserId;
        public string RecipeKey;
        public string RecipeName;
        public float? Price;
        public float? PricePerUnit;
        public float? CostRatio;

        public override string GetQuery()
        {
            return string.Format("?RecipeKey={0}", RecipeKey);
        }

        public override object GetParameter()
        {
            return this;
        }
    }
    public class RecipeDetailParam : Param
    {
        public string UserId;
        public string RecipeKey;
        public string RecipeName;
        public string RecipeDetailKey;
        public int Seq;
        public string IngredientKey;
        public string IngredientName;
        public float? MaterialUsage;
        public string UnitKey;
        public string UnitName;

        public override string GetQuery()
        {
            return string.Format("?RecipeKey={0}&RecipeDetailKey={1}", RecipeKey, RecipeDetailKey);
        }

        public override object GetParameter()
        {
            return this;
        }
    }
    public class RegisterParam : Param
    {
        public string UserId;
        public string Password;
        public string Name;
        public string Email;

        public override object GetParameter()
        {
            return this;
        }
    }

    public class LoginParam : Param
    {
        public string UserId;
        public string Password;

        public override object GetParameter()
        {
            return this;
        }
    }

    public class FindPasswordParam : Param
    {
        public string UserId;
        public string Email;

        public override object GetParameter()
        {
            return this;
        }
    }
    public class EmailParam : Param
    {
        public string Email;

        public override object GetParameter()
        {
            return this;
        }
    }
    public class VerifyCodeParam : Param
    {
        public string VerifyCode;

        public override string GetQuery()
        {
            return string.Format("?VerifyCode={0}", VerifyCode);
        }
        public override object GetParameter()
        {
            return this;
        }
    }
    public class GetResponse
    {
        public object result;
        public string message;
        public bool state;
    }
    public class PostResponse
    {
        public string message;
        public bool state;
    }

    public class ErrorResponse
    {
        public string timestamp;
        public int status;
        public string error;
        public string path;
    }
}
