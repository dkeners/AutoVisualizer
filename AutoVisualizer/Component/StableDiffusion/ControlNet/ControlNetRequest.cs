// https://github.com/Mikubill/sd-webui-controlnet/wiki/API
// Working in API v1

using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;

namespace AutoVisualizer.Component.StableDiffusion.ControlNet
{
    public class ControlNetRequest : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the ControlNetRequest class.
        /// </summary>
        public ControlNetRequest()
          : base("ControlNetRequest", "CtrlNet",
              "Creates ControlNet request JSON for the \"alwayson_scripts\" generation settings",
              "AutoVisualizer", "ControlNet")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Image", "Img", "Bitmap image for control", GH_ParamAccess.item);
            pManager.AddGenericParameter("mask", "m", "mask pixel_perfect to filter the image", GH_ParamAccess.item); // <== Unsure of data type (Bitmap?)
            pManager.AddTextParameter("module", "m", "module name", GH_ParamAccess.item, "None");
            pManager.AddTextParameter("model", "m", "model name", GH_ParamAccess.item, "None");
            pManager.AddNumberParameter("weight", "w", "weight", GH_ParamAccess.item, 1.0);
            pManager.AddIntegerParameter("resize_mode", "rm", "0: Just Resize, 1: Scale to Fit (Inner Fit), 3: Envelope (Outer Fit)", GH_ParamAccess.item, 1);
            pManager.AddBooleanParameter("lowvram", "lv", "low vram mode", GH_ParamAccess.item, false);
            pManager.AddIntegerParameter("processor_res", "pr", "processor resolution", GH_ParamAccess.item, 64);
            pManager.AddIntegerParameter("threshold_a", "ta", "threshold a", GH_ParamAccess.item, 64);
            pManager.AddIntegerParameter("threshold_b", "tb", "threshold b", GH_ParamAccess.item, 64);
            pManager.AddNumberParameter("guidance_start", "gs", "guidance start", GH_ParamAccess.item, 0.0);
            pManager.AddNumberParameter("guidance_end", "ge", "guidance end", GH_ParamAccess.item, 1.0);
            pManager.AddIntegerParameter("contol_mode", "cm", "0: Balanced, 1: My prompt is more important, 2: ControlNet is more important", GH_ParamAccess.item, 0);
            pManager.AddBooleanParameter("pixel_perfect", "pp", "pixel perfect mode", GH_ParamAccess.item, false);

            // Set all optional
            for (int i = 0; i < pManager.ParamCount; i++)
            {
                if (i == 0 || i == 2 || i == 3)
                {
                    continue;
                }

                pManager[i].Optional = true;
            }
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("ControlNetRequest", "CNR", "ControlNetRequest", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            // Declare a variable for the input String
            string module = "None";
            string model = "None";
            double weight = 1.0;
            int resize_mode = 1;
            bool lowvram = false;
            int processor_res = 64;
            int threshold_a = 64;
            int threshold_b = 64;
            double guidance_start = 0.0;
            double guidance_end = 1.0;
            int contol_mode = 0;
            bool pixel_perfect = false;
    
            // Reference the input String variable and assign the input String to it.
            DA.GetData(2, ref module);
            DA.GetData(3, ref model);
            DA.GetData(4, ref weight);
            DA.GetData(5, ref resize_mode);
            DA.GetData(6, ref lowvram);
            DA.GetData(7, ref processor_res);
            DA.GetData(8, ref threshold_a);
            DA.GetData(9, ref threshold_b);
            DA.GetData(10, ref guidance_start);
            DA.GetData(11, ref guidance_end);
            DA.GetData(12, ref contol_mode);
            DA.GetData(13, ref pixel_perfect);
    
            // Declare a variable for the input Bitmap
            System.Drawing.Bitmap image = null;
    
            // Reference the input Bitmap variable and assign the input Bitmap to it.
            DA.GetData(0, ref image);
    
            // Declare a variable for the input Bitmap
            System.Drawing.Bitmap mask = null;
    
            // Reference the input Bitmap variable and assign the input Bitmap to it.
            DA.GetData(1, ref mask);
    
            // Declare a variable for the output String
            string controlNetRequest = "";
    
            // Do something with the input. 
            // If the input is null...
            if (image == null)
            {
                // ...then display a message in the component's message area
                AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, "Please input a Bitmap");
            }
            else
            {
                // ...otherwise create a JSON object of all the input parameters and the input Bitmap in Base64
                Dictionary<string, object> controlNetRequestDict = new Dictionary<string, object>();
                controlNetRequestDict.Add("module", module);
                controlNetRequestDict.Add("model", model);
                controlNetRequestDict.Add("weight", weight);
                controlNetRequestDict.Add("resize_mode", resize_mode);
                controlNetRequestDict.Add("lowvram", lowvram);
                controlNetRequestDict.Add("processor_res", processor_res);
                controlNetRequestDict.Add("threshold_a", threshold_a);
                controlNetRequestDict.Add("threshold_b", threshold_b);
                controlNetRequestDict.Add("guidance_start", guidance_start);
                controlNetRequestDict.Add("guidance_end", guidance_end);
                controlNetRequestDict.Add("contol_mode", contol_mode);
                controlNetRequestDict.Add("pixel_perfect", pixel_perfect);
                controlNetRequestDict.Add("image", Convert.ToBase64String((byte[])new System.Drawing.ImageConverter().ConvertTo(image, typeof(byte[]))));
                controlNetRequestDict.Add("mask", Convert.ToBase64String((byte[])new System.Drawing.ImageConverter().ConvertTo(mask, typeof(byte[]))));

                // ...and convert the JSON object to a String in the format of: "controlnet": {
                //                                                                  "args": [
                 //                                                                         {
                    //                                                                      "module": "depth",
          //                                                                                "model": "diff_control_sd15_depth_fp16 [978ef0a1]"
                  //                                                                        }
     //                                                                                      ]
   //                                                                                        }
                const string controlNetRequestFormat = "{{ \"controlnet\": {{ \"args\": [ {0} ] }} }}";

                controlNetRequest = Newtonsoft.Json.JsonConvert.SerializeObject(controlNetRequestDict);

                controlNetRequest = string.Format(controlNetRequestFormat, controlNetRequest);
            }
            // print the output String to the component's message area
            AddRuntimeMessage(GH_RuntimeMessageLevel.Remark, (string)controlNetRequest);

            // Set the output to the output String variable
            DA.SetData(0, controlNetRequest);
        }

        /// <summary>
        /// Provides an Icon for the component.
        /// </summary>
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                //You can add image files to your project resources and access them like this:
                return Properties.Resources.ControlNetRequest;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("F31A5326-FB30-45D4-83EE-BC298D9EACE2"); }
        }
    }
}