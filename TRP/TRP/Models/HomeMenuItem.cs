using System;
using System.Collections.Generic;
using System.Text;

namespace TRP.Models
{
    public enum MenuItemType
    {
        Browse,
        About,
        Game
    }
    public class HomeMenuItem
    {
        public MenuItemType Id { get; set; }

        public string Title { get; set; }
    }
}
