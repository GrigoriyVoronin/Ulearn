using System;
using System.Windows.Forms;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace MyPhotoshop
{
	public class MainWindow : Form
	{
		Bitmap originalBmp;
        Photo originalPhoto;
		PictureBox original;
		PictureBox processed;
		ComboBox filtersSelect;
		Panel parametersPanel;
		List<NumericUpDown> parametersControls;
		Button apply;
		
		public MainWindow ()
		{
			original=new PictureBox();
			Controls.Add (original);
			
            processed=new PictureBox();
			Controls.Add(processed);
			
            filtersSelect=new ComboBox();
			filtersSelect.DropDownStyle = ComboBoxStyle.DropDownList;
			filtersSelect.SelectedIndexChanged+=FilterChanged;
			Controls.Add (filtersSelect);

            apply=new Button();
			apply.Text="Применить";
			apply.Enabled=false;
			apply.Click+=Process;
			Controls.Add (apply);

            Text="Photoshop pre-alpha release";
			FormBorderStyle = FormBorderStyle.FixedDialog;

            

            LoadBitmap((Bitmap)Image.FromFile("cat.jpg"));
		}

        public void LoadBitmap(Bitmap bmp)
        {
            originalBmp = bmp;
            originalPhoto = Convertors.Bitmap2Photo(bmp);

            original.Image = originalBmp;
            original.Left = 0;
            original.Top = 0;
            original.ClientSize = originalBmp.Size;

            processed.Left = 0;
            processed.Top = original.Bottom;
            processed.Size = original.Size;

            filtersSelect.Left = original.Right + 10;
            filtersSelect.Top = 20;
            filtersSelect.Width = 200;
            filtersSelect.Height = 20;


            ClientSize = new Size(filtersSelect.Right + 20, processed.Bottom);

            apply.Left = ClientSize.Width - 120;
            apply.Top = ClientSize.Height - 50;
            apply.Width = 100;
            apply.Height = 40;

            FilterChanged(null, EventArgs.Empty);
        }

        		
		public void AddFilter(IFilter filter)
		{
			filtersSelect.Items.Add(filter);
			if (filtersSelect.SelectedIndex==-1)
			{
				filtersSelect.SelectedIndex=0;
				apply.Enabled=true;
			}
		}
		
		void FilterChanged(object sender, EventArgs e)
		{
			var filter=(IFilter)filtersSelect.SelectedItem;
			if (filter==null) return;
			if (parametersPanel!=null) Controls.Remove (parametersPanel);
			parametersControls=new List<NumericUpDown>();
			parametersPanel=new Panel();
			parametersPanel.Left=filtersSelect.Left;
			parametersPanel.Top=filtersSelect.Bottom+10;
			parametersPanel.Width=filtersSelect.Width;
			parametersPanel.Height=ClientSize.Height-parametersPanel.Top;
			
			int y=0;
			
			foreach(var param in filter.GetParameters ())
			{
				var label=new Label();
				label.Left=0;
				label.Top=y;
				label.Width=parametersPanel.Width-50;
				label.Height=20;
				label.Text=param.Name;
				parametersPanel.Controls.Add (label);
				
				var box=new NumericUpDown();
				box.Left=label.Right;
				box.Top=y;
				box.Width=50;
				box.Height=20;
				box.Value=(decimal)param.DefaultValue;
				box.Increment=(decimal)param.Increment/3;
				box.Maximum=(decimal)param.MaxValue;
				box.Minimum=(decimal)param.MinValue;
                box.DecimalPlaces = 2;
				parametersPanel.Controls.Add (box);
				y+=label.Height+5;
				parametersControls.Add(box);
			}
			Controls.Add (parametersPanel);
		}
		
		
		void Process(object sender, EventArgs empty)
		{
			var data=parametersControls.Select(z=>(double)z.Value).ToArray();
			var filter=(IFilter)filtersSelect.SelectedItem;
			Photo result=null;
     		result=filter.Process(originalPhoto,data);
	        var resultBmp=Convertors.Photo2Bitmap(result);
			if (resultBmp.Width>originalBmp.Width || resultBmp.Height>originalBmp.Height)
			{
                float k = Math.Min((float)originalBmp.Width / resultBmp.Width, (float)originalBmp.Height / resultBmp.Height);
                var newBmp = new Bitmap((int)(resultBmp.Width * k), (int)(resultBmp.Height * k));
				using(var g = Graphics.FromImage(newBmp))
				{
					g.DrawImage(resultBmp, new Rectangle(0, 0, newBmp.Width, newBmp.Height), new Rectangle(0, 0, resultBmp.Width, resultBmp.Height), GraphicsUnit.Pixel);
				}
				resultBmp = newBmp;
			}
				
			processed.Image=resultBmp;
		}

        
	}
}
