using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Windows;
using System.Collections.ObjectModel;

namespace demo {
	public class GifType : DataTemplate {
		private String name;
		private ObservableCollection<String> gifs;
		private String desc;
		public String Name {
			get { return name; }
		}
		public ObservableCollection<String> GIFs {
			get { return gifs; }
		}
		public ObservableCollection<String> MainGifs {
			get {
				ObservableCollection<String> output = new ObservableCollection<String>(gifs);
				output.RemoveAt(0);
				return output;
			}
		}

		public String Description {
			get {
				return desc;
			}
            set
            {
                // not allowed
            }
		}
		public GifType(string name, string desc) {
			this.name = name;
			this.desc = desc;
			this.gifs = new ObservableCollection<String>();
		}

        public override String ToString()
        {
            return Name + ": " + Description;
        }

	}
}
