using System.Collections.Generic;
using System.Collections;
using System.Web;
using System.Web.Mvc;

public class FakeHttpContext : HttpContextBase
{
    private Dictionary<object, object> _items = new Dictionary<object, object>();
    public override IDictionary Items { get { return _items; } }
}

public class FakeViewDataContainer : IViewDataContainer

{

    private ViewDataDictionary _viewData = new ViewDataDictionary();

    public ViewDataDictionary ViewData { get { return _viewData; } set { _viewData = value; } }

}