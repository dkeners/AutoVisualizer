using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using AutoVisualizer.Utils;
using Grasshopper.Kernel;
using Rhino.Geometry;

namespace AutoVisualizer.Component.StableDiffusion
{
    public class GHC_SD_GenerateASync : Templates.GH_Component_ASync
    {
        private string prompt = "";
        private string negPrompt = "";
        private dynamic request_data;
        private List<string> outputPath = new List<string>();
        private List<Image> image = new List<Image>();

        /// <summary>
        /// Initializes a new instance of the MyComponent1 class.
        /// </summary>
        public GHC_SD_GenerateASync()
          : base("SDGenerateASync", "SD GenASync",
              "Generate image from Automatic1111 with asynchronous POST. Best generation method.",
              "AutoVisualizer", "Stable Diffusion")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("Prompt", "P", "Prompt for SD generation", GH_ParamAccess.item);
            pManager.AddTextParameter("Negative Prompt", "NP", "Negative Prompt for SD generation", GH_ParamAccess.item);
            pManager.AddTextParameter("Options", "O", "Options for SD generation", GH_ParamAccess.item, "");
            pManager.AddBooleanParameter("Generate", "G", "Generate image", GH_ParamAccess.item, false);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("Path", "P", "Output path of tmp image file", GH_ParamAccess.list);
            pManager.AddGenericParameter("Image", "I", "A bitmap object", GH_ParamAccess.list);
        }

        /// <summary>
        /// Updates the message displayed on the component before it is solved.
        /// </summary>
        protected override void BeforeSolveInstance()
        {
            base.BeforeSolveInstance();
            //UpdateMessage(message);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            if (this.shouldExpire)
            {
                switch (currentState)
                {
                    case RequestState.Off:
                        this.Message = "Inactive";
                        currentState = RequestState.Ready;
                        break;
                    case RequestState.Completed:
                        this.Message = "Image generated!";
                        AddRuntimeMessage(GH_RuntimeMessageLevel.Remark, $"Image generated!");
                        ProcessData();
                        currentState = RequestState.Ready;
                        break;
                    case RequestState.Failed:
                        this.Message = "Failed to generate image.";
                        currentState = RequestState.Ready;
                        break;
                }
                if (this.outputPath.Count > 1)
                {
                    DA.SetDataList(0, this.outputPath);
                    DA.SetDataList(1, this.image);
                }
                else if (this.outputPath.Count == 1)
                {
                    DA.SetData(0, this.outputPath[0]);
                    DA.SetData(1, this.image[0]);
                }
                else
                {
                    DA.SetData(0, "");
                    DA.SetData(1, null);
                }
                shouldExpire = false;

                return;
            }
            
            outputPath = new List<string>();
            image = new List<Image>();

            //CollectData();
            //Attributes.InvalidateCanvas();
            //GH_Canvas.Invalidate();

            DA.GetData(0, ref this.prompt);

            DA.GetData(1, ref this.negPrompt);

            string options = null;
            DA.GetData(2, ref options);

            var base_url = "http://127.0.0.1:7860";

            var routes = new
            {
                txt2img = new
                {
                    url = $"{base_url}/sdapi/v1/txt2img",
                    headers = new { accept = "application/json", Content_Type = "application/json" },
                },
                options = new
                {
                    url = $"{base_url}/sdapi/v1/options",
                    headers = new { accept = "application/json" },
                },
            };

            SetOptions(options);

            string routeName = "txt2img";
            dynamic route = routes.GetType().GetProperty(routeName)?.GetValue(routes);

            if (route != null)
            {
                string url = route.url;
                dynamic headers = route.headers;

                var data = this.request_data.GetType().GetProperty(routeName)?.GetValue(this.request_data) ?? new { };

                var dataJSON = Newtonsoft.Json.JsonConvert.SerializeObject(data);
                var headersJSON = Newtonsoft.Json.JsonConvert.SerializeObject(headers);

                currentState = RequestState.Running;
                this.Message = "Generating Image...";

                AsyncPost(url, dataJSON, headersJSON, "");
            }
            else
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, $"Route '{routeName}' not found.");
                currentState = RequestState.Failed;
            }
        }

        private void ProcessData()
        {
            if (this.responseData.Length > 0)
            {
                dynamic responseJSON = Newtonsoft.Json.JsonConvert.DeserializeObject(this.responseData);
                //AddRuntimeMessage(GH_RuntimeMessageLevel.Remark, $"Data: {responseJSON}");

                //var length = responseJSON.Length;
                var images = responseJSON["images"];

                if (images.Count == 1)
                {
                    string base64ImageData = images[0].ToString();

                    if (!string.IsNullOrEmpty(base64ImageData))
                    {
                        byte[] imageBytes = Convert.FromBase64String(base64ImageData);
                        using (MemoryStream stream = new MemoryStream(imageBytes))
                        {
                            this.image.Add(Image.FromStream(stream));
                            this.outputPath.Add("C:\\tmp\\output.png");
                            this.image[0].Save(outputPath[0]);

                            //DA.SetData(0, outputPath);
                            //AddRuntimeMessage(GH_RuntimeMessageLevel.Remark, $"Image saved to {outputPath}");

                            //message = "Image generated!";
                            //UpdateMessage(message);
                        }
                    }
                    else
                    {
                        AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "No image data in response.");
                        message = "No image data in response.";
                        currentState = RequestState.Failed;
                        UpdateMessage(message);
                    }
                }
                else if (images.Count > 1)
                {
                    for (int i = 0; i < images.Count; i++)
                    {
                        byte[] imageBytes = Convert.FromBase64String(images[i].ToString());
                        using (MemoryStream stream = new MemoryStream(imageBytes))
                        {
                            this.image.Add(Image.FromStream(stream));
                            this.outputPath.Add("C:\\tmp\\output" + i + ".png");
                            this.image[i].Save(outputPath[i]);

                            //DA.SetData(0, outputPath);
                            //AddRuntimeMessage(GH_RuntimeMessageLevel.Remark, $"Image saved to {outputPath}");

                            //message = "Image generated!";
                            //UpdateMessage(message);
                        }
                    }
                }
                else
                {
                    AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "No image data in response.");
                    message = "No image data in response.";
                    currentState = RequestState.Failed;
                    UpdateMessage(message);
                }
            }
            else
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "No response data.");
                message = "No response data.";
                currentState = RequestState.Failed;
                UpdateMessage(message);
            }
        }

        private void SetOptions(string options)
        {
            if (options != "")
            {
                dynamic optionsData = Newtonsoft.Json.JsonConvert.DeserializeObject(options);
                optionsData.prompt = this.prompt;
                optionsData.negative_prompt = this.negPrompt;
                this.request_data = new
                {
                    txt2img = (object)optionsData,
                    img2img = new
                    {
                        // Define img2img request parameters here
                    },
                };
            }
            else
            {
                this.request_data = new
                {
                    txt2img = new
                    {
                        this.prompt,
                        negative_prompt = this.negPrompt,
                        seed = -1,
                        sampler_name = "Euler a",
                        batch_size = 1,
                        n_iter = 1,
                        steps = 50,
                        cfg_scale = 7,
                        width = 512,
                        height = 512,
                        restore_faces = true,
                        do_not_save_samples = false,
                        do_not_save_grid = false,
                        // Add other txt2img request parameters here
                    },
                    img2img = new
                    {
                        // Define img2img request parameters here
                    },
                };
            }
        }

        /// <summary>
        /// Provides an Icon for the component.
        /// </summary>
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return Properties.Resources.SDGenerateASync;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("fceb9093-b213-410b-9f10-d7235865c1c4"); }
        }
    }
}