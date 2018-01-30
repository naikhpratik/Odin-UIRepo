﻿using System;
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
        if (name == "time")
        {
            icon = "time";
        }
        string res = "";
        TagBuilder tagDiv = new TagBuilder("div");
        tagDiv.AddCssClass("input-group " + clss);
        res = tagDiv.ToString(TagRenderMode.StartTag);

        TagBuilder tagSimInput = new TagBuilder("div");
        tagSimInput.AddCssClass("form-control-div");
        tagSimInput.Attributes.Add("placeholder", "Enter " + clss + " or click");
        res += tagSimInput.ToString(TagRenderMode.StartTag) + "</div>";

        TagBuilder tagInput = new TagBuilder("input");
        tagInput.AddCssClass("form-control");
        tagInput.Attributes.Add("type", "text");
        tagInput.Attributes.Add("name", name);

        if (includePlaceHolder)
        {
            tagInput.Attributes.Add("placeholder", "Enter " + clss + " or click");
        }
        tagInput.Attributes.Add("data-date-today-highlight", "true");
        tagInput.Attributes.Add("data-date-today-btn", "true");
        tagInput.Attributes.Add("readonly", "readonly");
        res += tagInput.ToString(TagRenderMode.SelfClosing);
        TagBuilder tagSpan1 = new TagBuilder("span");
        tagSpan1.AddCssClass("input-group-addon");
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
        if (clss == "time")
        {
            icon = "time";
        }
        string res = "";
        TagBuilder tagDiv = new TagBuilder("div");
        tagDiv.AddCssClass("input-group " + clss);
        res = tagDiv.ToString(TagRenderMode.StartTag);

        TagBuilder tagInput = new TagBuilder("input");
        tagInput.Attributes.Add("name", name);
        tagInput.AddCssClass("form-control");
        tagInput.Attributes.Add("type", "text");
        if (includePlaceHolder)
        {
            tagInput.Attributes.Add("placeholder", "Enter " + clss + " or click");
        }
        tagInput.Attributes.Add("data-date-today-highlight", "true");
        tagInput.Attributes.Add("data-date-today-btn", "true");
        tagInput.Attributes.Add("readonly", "readonly");
        tagInput.Attributes.Add("value", value.ToString("dd-MMM-yyyy hh:mm tt"));
        res += tagInput.ToString(TagRenderMode.SelfClosing);
        TagBuilder tagSpan1 = new TagBuilder("span");
        tagSpan1.AddCssClass("input-group-addon");
        res += tagSpan1.ToString(TagRenderMode.StartTag);
        TagBuilder tagSpan2 = new TagBuilder("span");
        tagSpan2.AddCssClass("glyphicon glyphicon-" + icon);
        res += tagSpan2.ToString(TagRenderMode.Normal) + "</span></div>";
        HtmlString html = new HtmlString(res);
        return html;
    }

    public static IHtmlString DatePicker(this HtmlHelper htmlHelper, string clss, string name, string dateValue)
    {
        if (String.IsNullOrEmpty(clss) || String.IsNullOrEmpty(name))
            throw new ArgumentNullException("helper");

        string icon = "calendar";
        if (clss == "time")
        {
            icon = "time";
        }
        string res = "";
        TagBuilder tagDiv = new TagBuilder("div");
        tagDiv.AddCssClass("input-group " + clss);
        res = tagDiv.ToString(TagRenderMode.StartTag);

        TagBuilder tagInput = new TagBuilder("input");
        tagInput.Attributes.Add("name", name);
        tagInput.AddCssClass("form-control");
        tagInput.Attributes.Add("placeholder", "Enter " + clss + " or click");
        tagInput.Attributes.Add("data-date-today-highlight", "true");
        tagInput.Attributes.Add("data-date-today-btn", "true");
        tagInput.Attributes.Add("readonly", "readonly");
        tagInput.Attributes.Add("value", dateValue);
        tagInput.Attributes.Add("type", "text");

        res += tagInput.ToString(TagRenderMode.SelfClosing);
        TagBuilder tagSpan1 = new TagBuilder("span");
        tagSpan1.AddCssClass("input-group-addon");
        res += tagSpan1.ToString(TagRenderMode.StartTag);
        TagBuilder tagSpan2 = new TagBuilder("span");
        tagSpan2.AddCssClass("glyphicon glyphicon-" + icon);
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
        if (name == "time")
        {
            icon = "time";
        }
        string res = "";
        TagBuilder tagDiv = new TagBuilder("div");
        tagDiv.Attributes.Add("name", name);
        res = tagDiv.ToString(TagRenderMode.StartTag);

        TagBuilder tagSimInput = new TagBuilder("div");
        tagSimInput.AddCssClass("form-control-div");
        tagSimInput.Attributes.Add("placeholder", "Enter date/time or click");
        res += tagSimInput.ToString(TagRenderMode.StartTag) + "</div>";

        TagBuilder tagInput = new TagBuilder("input");
        tagInput.AddCssClass("form-control");
        tagInput.Attributes.Add("type", "text");
        tagInput.Attributes.Add("placeholder", "Enter " + name + " or click");
        tagInput.Attributes.Add("data-date-today-highlight", "true");
        tagInput.Attributes.Add("data-date-today-btn", "true");
        tagInput.Attributes.Add("readonly", "readonly");
        foreach (string key in validationAttributes.Keys)
        {
            tagInput.Attributes.Add(key, validationAttributes[key].ToString());
        }
        res += tagInput.ToString(TagRenderMode.SelfClosing);
        TagBuilder tagSpan1 = new TagBuilder("span");
        tagSpan1.AddCssClass("input-group-addon");
        res += tagSpan1.ToString(TagRenderMode.StartTag);
        TagBuilder tagSpan2 = new TagBuilder("span");
        tagSpan2.AddCssClass("glyphicon glyphicon-" + icon);
        res += tagSpan2.ToString(TagRenderMode.Normal) + "</span></div>";
        return res;
    }
}