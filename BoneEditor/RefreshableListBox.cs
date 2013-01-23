using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace BoneEditor
{
    /// <summary>
    /// Listbox that allow other classees to refresh its values.
    /// </summary>
    class RefreshableListBox : ListBox
    {
        public new void RefreshItems()
        {
            base.RefreshItems();
        }
        public new void RefreshItem(int index)
        {
            base.RefreshItem(index);
        }
    }
}
