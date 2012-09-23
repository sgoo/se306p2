using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace demo {
    public class GifType : DataTemplate {
        private string name;
        private string[] gifs;
        public string Name {
            get { return name; }
        }
        public string[] GIFs {
            get { return gifs; }
        }
        public GifType(string name, string[] gifs) {
            this.name = name;
            this.gifs = gifs;
        }
    }
}
