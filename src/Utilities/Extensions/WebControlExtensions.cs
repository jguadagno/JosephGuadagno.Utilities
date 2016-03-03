using System.Web.UI;
using System.Web.UI.WebControls;

namespace JosephGuadagno.Utilities.Extensions
{
    public static class WebControlExtensions
    {
        public static T FindControl<T>(this Control control, string id) where T : class
        {
            return control.FindControl(id) as T;
        }

        public static void SetDropDownValue(this DropDownList ddl, int value)
        {
            var item = ddl.Items.FindByValue(value.ToString());
            if (item != null)
                ddl.SelectedIndex = ddl.Items.IndexOf(item);
        }
    }
}