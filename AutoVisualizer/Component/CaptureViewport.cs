using System;
using System.Collections.Generic;
using AutoVisualizer.Utils;
using Grasshopper.Kernel;
using Rhino.Display;
using Rhino.Geometry;

namespace AutoVisualizer.Component
{
    public class CaptureViewport : GH_Component
    {
        private string _imagePath = null;
        private ButtonRunCheck _buttonCheck = new ButtonRunCheck();
        
        /// <summary>
        /// Initializes a new instance of the CaptureViewport class.
        /// </summary>
        public CaptureViewport()
          : base("CaptureViewport", "CaptView",
              "Capture the current viewport.",
              "AutoVisualizer", "Capture")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("Path", "P", "Path to save the image file.", GH_ParamAccess.item);
            // make this input optional
            pManager[0].Optional = true;

            pManager.AddTextParameter("Options", "O", "Options for the capture.", GH_ParamAccess.item);
            pManager[1].Optional = true;

            pManager.AddBooleanParameter("Capture", "C", "Capture the viewport. Use with normal button component.", GH_ParamAccess.item, false);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Bitmap Image", "BI", "Bitmap Image data.", GH_ParamAccess.item);
            pManager.AddTextParameter("Image Path", "IP", "Path of saved image file.", GH_ParamAccess.item);
        }

        //protected override void BeforeSolveInstance()
        //{
        //    if ( )
        //    {

        //    }
        //    if (Params.Input[1].SourceCount == 0)
        //    {
        //        _buttonState = ButtonState.Unclicked;
        //    }
        //    else
        //    {
        //        bool buttonClicked = false;
        //        Params.Input[1].CollectData();
        //        Params.Input[1].VolatileData.AllData(true);
        //        foreach (var item in Params.Input[1].VolatileData.AllData(true))
        //        {
        //            if (item is bool)
        //            {
        //                buttonClicked = (bool)item;
        //            }
        //        }

        //        if (buttonClicked)
        //        {
        //            _buttonState = ButtonState.Clicked;
        //        }
        //        else
        //        {
        //            _buttonState = ButtonState.Unclicked;
        //        }
        //    }
        //}

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            bool buttonInput = false;
            DA.GetData<bool>(2, ref buttonInput);

            if (buttonInput)
            {
                _buttonCheck.SetButtonState(ButtonState.Clicked);
                return;
            }
            if (_buttonCheck.IsClicked() && !buttonInput)
            {
                _buttonCheck.Reset();
            }

            DA.GetData<string>(0, ref _imagePath);
            if (_imagePath == null)
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, "No path specified, setting: \"C:\\\\tmp\\\\ViewportCapture.png\"");
                _imagePath = "C:\\tmp\\ViewportCapture.png";
            }
            else if (!_imagePath.EndsWith(".png"))
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, "Path does not end with .png, changing to .png");
                _imagePath += ".png";
            }

            // Says not to use ActiveDoc, but it's the only way to get the current viewport. https://developer.rhino3d.com/api/rhinocommon/rhino.rhinodoc/activedoc
            Rhino.Display.RhinoView view = Rhino.RhinoDoc.ActiveDoc.Views.ActiveView;

            if (view == null)
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "No active view.");
                return;
            }

            System.Drawing.Bitmap image;
            string options = null;
            DA.GetData<string>(1, ref options);

            if (options != null)
            {
                dynamic optionsObj = Newtonsoft.Json.JsonConvert.DeserializeObject(options);
                image = view.CaptureToBitmap((bool)optionsObj["grid"], (bool)optionsObj["worldAxes"], (bool)optionsObj["cplaneAxes"]);
            }
            else
            {
                image = view.CaptureToBitmap(true, true, true);
            }
            

            // Save the bitmap to a file.
            image.Save(_imagePath, System.Drawing.Imaging.ImageFormat.Png);

            DA.SetData(0, image);
            DA.SetData(1, _imagePath);
        }

        /// <summary>
        /// Provides an Icon for the component.
        /// </summary>
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                //You can add image files to your project resources and access them like this:
                return Properties.Resources.CaptureViewport;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("F2B6382E-0E12-47EA-8069-8813D7106114"); }
        }
    }
}