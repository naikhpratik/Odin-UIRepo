using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;

public static class DatePickerHelper
{
        //For calling the helper that takes a model as a parameter                
        //use the following code    
            //@*@Html.DatePicker("svcPreArrival")
            //@Html.DatePickerFor(m => m.BirthDate)
            //@Html.ValidationMessageFor(m => m.BirthDate)*@
        public static IHtmlString DatePickerFor<TModel, TProperty>(this HtmlHelper<TModel> helper, Expression<Func<TModel, TProperty>> expression)
        {
            string datePickerName = ExpressionHelper.GetExpressionText(expression);
            string datePickerFullName = helper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(datePickerName);
            string datePickerID = TagBuilder.CreateSanitizedId(datePickerFullName);

            ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, helper.ViewData);
            DateTime datePickerValue = (metadata.Model == null ? DateTime.Now : DateTime.Parse(metadata.Model.ToString()));
            IDictionary<string, object> validationAttributes = helper.GetUnobtrusiveValidationAttributes(datePickerFullName, metadata);
            string tag = buildTags(datePickerFullName, validationAttributes);

            MvcHtmlString html = new MvcHtmlString(tag);
            return html;
        }

        public static IHtmlString DatePicker(this HtmlHelper htmlhelper, string clss, string name, bool includePlaceHolder = true)
        {
            string icon = "calendar";
            //string wid = "100%";
            if (name == "time")
            {
                icon = "time";
              //  wid = "92%";
            }
            string res = "";
            TagBuilder tagDiv = new TagBuilder("div");
            tagDiv.AddCssClass("input-group " + clss);
            //tagDiv.Attributes.Add("style", "width:" + wid);
            res = tagDiv.ToString(TagRenderMode.StartTag);

            TagBuilder tagInput = new TagBuilder("input");
            tagInput.AddCssClass("form-control");
            tagInput.Attributes.Add("type", "text");
            tagInput.Attributes.Add("name", name);
           tagInput.Attributes.Add("style", "display:block;width:90%;");

            if (includePlaceHolder)
            {
                tagInput.Attributes.Add("placeholder", "Enter " + clss + " or hit " + icon);
            }
            tagInput.Attributes.Add("data-date-today-highlight", "true");
            tagInput.Attributes.Add("data-date-today-btn", "true");
        
            res += tagInput.ToString(TagRenderMode.SelfClosing);

            TagBuilder tagSpan1 = new TagBuilder("span");
            tagSpan1.AddCssClass("input-group-addon");
            tagSpan1.Attributes.Add("style", "display:block;");//width:15%

            res += tagSpan1.ToString(TagRenderMode.StartTag);

            TagBuilder tagSpan2 = new TagBuilder("span");
            tagSpan2.AddCssClass("glyphicon glyphicon-" + icon);
            tagSpan2.Attributes.Add("style", "text-align:left");

            res += tagSpan2.ToString(TagRenderMode.Normal) + "</span></div>";

            HtmlString html = new HtmlString(res);
            return html;
        }

        public static IHtmlString DatePicker(this HtmlHelper htmlHelper, string clss, string name, DateTime value, bool includePlaceHolder = true)
        {
            if (String.IsNullOrEmpty(clss) || String.IsNullOrEmpty(name))
                throw new ArgumentNullException("helper");

            string icon = "calendar";
            //string wid = "100%";
            if (clss == "time")
            {
                icon = "time";
               // wid = "92%";
            }
            string res = "";
            TagBuilder tagDiv = new TagBuilder("div");        
            tagDiv.AddCssClass("input-group " + clss);
            //tagDiv.Attributes.Add("style", "width:" + wid);        
            res = tagDiv.ToString(TagRenderMode.StartTag);

            TagBuilder tagInput = new TagBuilder("input");
            tagInput.Attributes.Add("name", name);
            tagInput.AddCssClass("form-control");            
            tagInput.Attributes.Add("type", "text");
            tagInput.Attributes.Add("style", "display:block;width:90%;");

            if (includePlaceHolder)
            {
                tagInput.Attributes.Add("placeholder", "Enter " + clss + " or hit " + icon);
            }
            tagInput.Attributes.Add("data-date-today-highlight", "true");
            tagInput.Attributes.Add("data-date-today-btn", "true");
            tagInput.Attributes.Add("value", value.ToString("dd-MMM-yyyy"));

            res += tagInput.ToString(TagRenderMode.SelfClosing);

            TagBuilder tagSpan1 = new TagBuilder("span");
            tagSpan1.AddCssClass("input-group-addon");
            tagSpan1.Attributes.Add("style", "display:block;");//width:15%

            res += tagSpan1.ToString(TagRenderMode.StartTag);

            TagBuilder tagSpan2 = new TagBuilder("span");
            tagSpan2.AddCssClass("glyphicon glyphicon-"+icon);
            tagSpan2.Attributes.Add("style", "text-align:left");

            res += tagSpan2.ToString(TagRenderMode.Normal) + "</span></div>";
                
            HtmlString html = new HtmlString(res);
            return html;
        }

    public static IHtmlString DatePicker(this HtmlHelper htmlHelper, string clss, string name, string dateValue)
    {
        if (String.IsNullOrEmpty(clss) || String.IsNullOrEmpty(name))
            throw new ArgumentNullException("helper");

        string icon = "calendar";
        //string wid = "100%";
        if (clss == "time")
        {
            icon = "time";
           // wid = "92%";
        }
        string res = "";
        TagBuilder tagDiv = new TagBuilder("div");
        tagDiv.AddCssClass("input-group " + clss);
        //tagDiv.Attributes.Add("style", "width:" + wid);
        res = tagDiv.ToString(TagRenderMode.StartTag);

        TagBuilder tagInput = new TagBuilder("input");
        tagInput.Attributes.Add("name", name);
        tagInput.AddCssClass("form-control");
        tagInput.Attributes.Add("type", "text");
        tagInput.Attributes.Add("style", "display:block;width:90%;");
        tagInput.Attributes.Add("placeholder", "Enter " + clss + " or hit " + icon);
        tagInput.Attributes.Add("data-date-today-highlight", "true");
        tagInput.Attributes.Add("data-date-today-btn", "true");
        tagInput.Attributes.Add("value", dateValue);

        res += tagInput.ToString(TagRenderMode.SelfClosing);

        TagBuilder tagSpan1 = new TagBuilder("span");
        tagSpan1.AddCssClass("input-group-addon");
        tagSpan1.Attributes.Add("style", "display:block;"); //width:15%

        res += tagSpan1.ToString(TagRenderMode.StartTag);

        TagBuilder tagSpan2 = new TagBuilder("span");
        tagSpan2.AddCssClass("glyphicon glyphicon-" + icon);
        tagSpan2.Attributes.Add("style", "text-align:left");

        res += tagSpan2.ToString(TagRenderMode.Normal) + "</span></div>";

        HtmlString html = new HtmlString(res);
        return html;
    }

    public static IHtmlString DatePicker(this HtmlHelper htmlHelper, string clss, string name, DateTime? dateValue, bool includePlaceHolder = false)
    {
        return dateValue.HasValue
            ? DatePicker(htmlHelper, clss, name, dateValue.Value, includePlaceHolder)
            : DatePicker(htmlHelper, clss, name, includePlaceHolder);
    }
    

    private static string buildTags(string name, IDictionary<string, object> validationAttributes)
        {
            string icon = "calendar";
            //string wid = "100%";
            if (name == "time")
            {
                icon = "time";
                //wid = "92%";
            }
            string res = "";
            TagBuilder tagDiv = new TagBuilder("div");
            tagDiv.Attributes.Add("name", name);
            //tagDiv.Attributes.Add("style", "width:" + wid);
            res = tagDiv.ToString(TagRenderMode.StartTag);

            TagBuilder tagInput = new TagBuilder("input");
            tagInput.AddCssClass("form-control");
            tagInput.Attributes.Add("type", "text");
            tagInput.Attributes.Add("style", "display:block;width:90%;");
            tagInput.Attributes.Add("placeholder", "Enter " + name + " or hit " + icon);
            tagInput.Attributes.Add("data-date-today-highlight", "true");
            tagInput.Attributes.Add("data-date-today-btn", "true");
            foreach (string key in validationAttributes.Keys)
            {
                tagInput.Attributes.Add(key, validationAttributes[key].ToString());
            }
            res += tagInput.ToString(TagRenderMode.SelfClosing);

            TagBuilder tagSpan1 = new TagBuilder("span");
            tagSpan1.AddCssClass("input-group-addon");
            tagSpan1.Attributes.Add("style", "display:block;");//width:15%

            res += tagSpan1.ToString(TagRenderMode.StartTag);

            TagBuilder tagSpan2 = new TagBuilder("span");
            tagSpan2.AddCssClass("glyphicon glyphicon-" + icon);
            tagSpan2.Attributes.Add("style", "text-align:left");

            res += tagSpan2.ToString(TagRenderMode.Normal) + "</span></div>";
            return res;
        }
    }
