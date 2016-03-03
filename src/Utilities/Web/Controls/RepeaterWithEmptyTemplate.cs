using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace JosephGuadagno.Utilities.Web.Controls
{
    /// <summary>
    ///     Advance ASP.NET Repeater with EmptyTemplate, ShowHeaderWhenEmpty, ShowFooterWhenEmpty
    /// </summary>
    [ToolboxData("<{0}:RepeaterWithEmptyTemplate runat=\"server\"><{0}:RepeaterWithEmptyTemplate")]
    public class RepeaterWithEmptyTemplate : Repeater
    {
        #region Repeater Properties

        /// <summary>
        ///     Gets or sets a value indicating whether [show header when empty].
        /// </summary>
        /// <value>
        ///     <c>true</c> if [show header when empty]; otherwise, <c>false</c>.
        /// </value>
        [PersistenceMode(PersistenceMode.Attribute)]
        public bool ShowHeaderWhenEmpty { get; set; }

        [PersistenceMode(PersistenceMode.Attribute)]
        public bool ShowFooterWhenEmpty { get; set; }

        /// <summary>
        ///     Gets or sets the empty template.
        /// </summary>
        /// <value>The empty template.</value>
        [PersistenceMode(PersistenceMode.InnerProperty)]
        public ITemplate EmptyTemplate { get; set; }

        /// <summary>
        ///     Gets or sets the empty template.
        /// </summary>
        /// <value>The empty template.</value>
        [PersistenceMode(PersistenceMode.Attribute)]
        public bool ShowCount { get; set; }

        #endregion

        #region Repeater Methods

        /// <summary>
        ///     Creates the child controls.
        /// </summary>
        protected override void CreateChildControls()
        {
            base.CreateChildControls();
            EnabledEmptyTemplate();
        }

        /// <summary>
        ///     Raises the DataBinding event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs" /> object that contains the event Domain.</param>
        protected override void OnDataBinding(EventArgs e)
        {
            base.OnDataBinding(e);
            EnabledEmptyTemplate();
            if (ShowCount)
            {
                Controls.Add(new LiteralControl("<br/>Total Number of Records : " +
                                                Items.Count));
            }
        }

        /// <summary>
        ///     Enableds the empty template.
        /// </summary>
        private void EnabledEmptyTemplate()
        {
            if (Items.Count <= 0 && EmptyTemplate != null)
            {
                Controls.Clear();

                // Instantiate Header Template When Item count 0
                if (ShowHeaderWhenEmpty)
                {
                    HeaderTemplate.InstantiateIn(this);
                }

                EmptyTemplate.InstantiateIn(this);
            }
        }

        #endregion
    }
}