﻿using System;
using System.IO;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Odin.Tests.Views.Helpers
{
    [TestClass]
    public class HTMLHelpersTests
    {

        private static HtmlHelper CreateMockHtmlHelper(ViewDataDictionary vd)
        {
            Mock<ViewContext> mockViewContext = new Mock<ViewContext>(
                new ControllerContext(
                    new Mock<HttpContextBase>().Object,
                    new RouteData(),
                    new Mock<ControllerBase>().Object
                ),
                new Mock<IView>().Object, vd, new TempDataDictionary(), new StreamWriter(new MemoryStream()));

            Mock<IViewDataContainer> mockDataContainer = new Mock<IViewDataContainer>();
            mockDataContainer.Setup(c => c.ViewData).Returns(vd);

            return new HtmlHelper(mockViewContext.Object, mockDataContainer.Object);
        }
   
        [TestMethod]
        public void DatePicker_should_render_HTML_input_with_date()
        {
            var vc = new ViewContext();
            vc.HttpContext = new FakeHttpContext();        
            var hh = new HtmlHelper(vc, new FakeViewDataContainer());
            var result = DatePickerHelper.DatePicker(hh, "pickerClass", "pickerName", DateTime.Now).ToString();
            Assert.IsTrue(result.Contains("<div class=\"input-group pickerClass\"") && result.Contains("value=\"" + DateTime.Now.ToString("dd-MMM-yyyy")));
        }
        [TestMethod]
        public void DatePicker_should_render_HTML_imput_with_name_only()
        {
            var vc = new ViewContext();
            vc.HttpContext = new FakeHttpContext();       
            var hh = new HtmlHelper(vc, new FakeViewDataContainer());
            var result = DatePickerHelper.DatePicker(hh, "pickerClass", "pickerName").ToString();
            Assert.IsTrue(result.Contains("<div class=\"input-group pickerClass\""));
        }
        //[TestMethod]
        //public void DatePicker_should_render_HTML_imput_from_model()
        //{
        //    var vc = new ViewContext();
        //    vc.HttpContext = new FakeHttpContext();       
        //    var hh = new HtmlHelper(vc, new FakeViewDataContainer());
        //    //TODO: create viewmodel 
        //    //var model = new vModel();
        //    var result = DatePickerHelper.DatePicker(hh, new Expression("model")).ToString();
        //    Assert.IsTrue(result.Contains("<input name=\"pickerName\""));
        //}
    }
}