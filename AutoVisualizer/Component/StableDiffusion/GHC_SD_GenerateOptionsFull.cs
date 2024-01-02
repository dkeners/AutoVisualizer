using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;

namespace AutoVisualizer.Component.StableDiffusion
{
    public class GHC_SD_GenerateOptionsFull : Templates.GH_Component_SD_Options
    {
        /// <summary>
        /// Initializes a new instance of the MyComponent1 class.
        /// </summary>
        public GHC_SD_GenerateOptionsFull()
          : base("   SDGenerateSettingsFull", "   SD GenSetF",
              "Configure all settings for Stable Diffusion image generation",
              "AutoVisualizer", "Stable Diffusion")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("Prompt", "P", "Prompt for SD generation", GH_ParamAccess.item);
            pManager.AddTextParameter("Negative Prompt", "NP", "Negative Prompt for SD generation", GH_ParamAccess.item);
            pManager.AddTextParameter("Styles", "St", "styles of the prompt", GH_ParamAccess.item);
            pManager.AddNumberParameter("Seed", "S", "Seed used for generation", GH_ParamAccess.item, -1);
            pManager.AddNumberParameter("Subseed", "Ss", "Subseed used for generation", GH_ParamAccess.item, -1);
            pManager.AddNumberParameter("Subseed Strength", "Ss-s", "Strength of Subseed in generation", GH_ParamAccess.item, 0);
            pManager.AddNumberParameter("Seed resize from h", "Sr-h", "Get similar results as image with height h", GH_ParamAccess.item, -1);
            pManager.AddNumberParameter("Seed resize from w", "Sr-w", "Get similar results as image with height w", GH_ParamAccess.item, -1);
            pManager.AddTextParameter("Sampler Name", "SN", "Sampler to use for image generation", GH_ParamAccess.item, "Euler a");
            // Add the remaining parameters
            pManager.AddIntegerParameter("batch_size", "BS", "Batch size", GH_ParamAccess.item, (int)1);
            pManager.AddIntegerParameter("n_iter", "NI", "Number of iterations", GH_ParamAccess.item, (int)1);
            pManager.AddIntegerParameter("steps", "ST", "Number of steps", GH_ParamAccess.item, (int)50);
            pManager.AddNumberParameter("cfg_scale", "CS", "CFG scale", GH_ParamAccess.item, 7.0);
            pManager.AddIntegerParameter("width", "W", "Image width", GH_ParamAccess.item, (int)512);
            pManager.AddIntegerParameter("height", "H", "Image height", GH_ParamAccess.item, (int)512);
            pManager.AddBooleanParameter("restore_faces", "RF", "Restore faces", GH_ParamAccess.item, true);
            pManager.AddTextParameter("tiling", "TL", "Tiling", GH_ParamAccess.item);
            pManager.AddBooleanParameter("do_not_save_samples", "DNSS", "Do not save samples", GH_ParamAccess.item, false);
            pManager.AddBooleanParameter("do_not_save_grid", "DNSG", "Do not save grid", GH_ParamAccess.item, false);

            pManager.AddTextParameter("eta", "ETA", "Eta", GH_ParamAccess.item);
            pManager.AddNumberParameter("denoising_strength", "DS", "Denoising strength", GH_ParamAccess.item, (double)0);

            pManager.AddTextParameter("s_min_uncond", "SMU", "S Min Uncond", GH_ParamAccess.item);
            pManager.AddTextParameter("s_churn", "SCH", "S Churn", GH_ParamAccess.item);
            pManager.AddTextParameter("s_tmax", "STM", "S T Max", GH_ParamAccess.item);
            pManager.AddTextParameter("s_tmin", "STMN", "S T Min", GH_ParamAccess.item);
            pManager.AddTextParameter("s_noise", "SN", "S Noise", GH_ParamAccess.item);
            pManager.AddTextParameter("override_settings", "OS", "Override Settings", GH_ParamAccess.item);
            pManager.AddBooleanParameter("override_settings_restore_afterwards", "OSRA", "Override Settings Restore Afterwards", GH_ParamAccess.item, true);

            pManager.AddTextParameter("refiner_checkpoint", "RC", "Refiner Checkpoint", GH_ParamAccess.item);

            pManager.AddNumberParameter("refiner_switch_at", "RSA", "Refiner Switch At", GH_ParamAccess.item);

            pManager.AddBooleanParameter("disable_extra_networks", "DEN", "Disable Extra Networks", GH_ParamAccess.item, false);

            pManager.AddTextParameter("comments", "C", "Comments", GH_ParamAccess.item);
            pManager.AddBooleanParameter("enable_hr", "EHR", "Enable HR", GH_ParamAccess.item, false);
            pManager.AddIntegerParameter("firstphase_width", "FPW", "First Phase Width", GH_ParamAccess.item, 0);
            pManager.AddIntegerParameter("firstphase_height", "FPH", "First Phase Height", GH_ParamAccess.item, 0);
            pManager.AddNumberParameter("hr_scale", "HRS", "HR Scale", GH_ParamAccess.item, 2.0);
            pManager.AddTextParameter("hr_upscaler", "HRU", "HR Upscaler", GH_ParamAccess.item);
            pManager.AddIntegerParameter("hr_second_pass_steps", "HRSPS", "HR Second Pass Steps", GH_ParamAccess.item, 0);
            pManager.AddIntegerParameter("hr_resize_x", "HRRX", "HR Resize X", GH_ParamAccess.item, 0);
            pManager.AddIntegerParameter("hr_resize_y", "HRRY", "HR Resize Y", GH_ParamAccess.item, 0);
            pManager.AddTextParameter("hr_checkpoint_name", "HRCN", "HR Checkpoint Name", GH_ParamAccess.item);
            pManager.AddTextParameter("hr_sampler_name", "HRSN", "HR Sampler Name", GH_ParamAccess.item);
            pManager.AddTextParameter("hr_prompt", "HRP", "HR Prompt", GH_ParamAccess.item, "");
            pManager.AddTextParameter("hr_negative_prompt", "HRNP", "HR Negative Prompt", GH_ParamAccess.item, "");
            pManager.AddTextParameter("sampler_index", "SI", "Sampler Index", GH_ParamAccess.item, "Euler");
            pManager.AddTextParameter("script_name", "SN", "Script Name", GH_ParamAccess.item);
            pManager.AddTextParameter("script_args", "SA", "Script Arguments", GH_ParamAccess.list, new List<string>());
            pManager.AddBooleanParameter("send_images", "SI", "Send Images", GH_ParamAccess.item, true);
            pManager.AddBooleanParameter("save_images", "SvI", "Save Images", GH_ParamAccess.item, false);
            pManager.AddTextParameter("alwayson_scripts", "AS", "Always On Scripts", GH_ParamAccess.item, "{}");

            for (int i = 0; i < Params.Input.Count; i++)
            {
                Params.Input[i].Optional = true;
            }
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("Generation Settings", "GS", "Settings for use in Stable Diffusion image generation.", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {

            // Retrieve input values using the helper method
            string prompt = GetInputValue(DA, 0, (string)null);
            string negPrompt = GetInputValue(DA, 1, (string)null);
            string styles = GetInputValue(DA, 2, (string)null);
            double seed = GetInputValue(DA, 3, (double)-1);
            double subseed = GetInputValue(DA, 4, (double)-1);
            double subseedStrength = GetInputValue(DA, 5, (double)0);
            double seedResizeFromH = GetInputValue(DA, 6, (double)-1);
            double seedResizeFromW = GetInputValue(DA, 7, (double)-1);
            string samplerName = GetInputValue(DA, 8, "Euler a");
            //AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "6");
            int batchSize = GetInputValue(DA, 9, 1);
            int nIter = GetInputValue(DA, 10, 1);
            int steps = GetInputValue(DA, 11, 50);
            double cfgScale = GetInputValue(DA, 12, (double)7.0);
            int width = GetInputValue(DA, 13, 512);
            int height = GetInputValue(DA, 14, 512);
            bool restoreFaces = GetInputValue(DA, 15, true);
            string tiling = GetInputValue(DA, 16, (string)null);
            bool doNotSaveSamples = GetInputValue(DA, 17, false);
            bool doNotSaveGrid = GetInputValue(DA, 18, false);
            string eta = GetInputValue(DA, 19, (string)null);
            double denoisingStrength = GetInputValue(DA, 20, (double)0);
            string sMinUncond = GetInputValue(DA, 21, (string)null);
            string sChurn = GetInputValue(DA, 22, (string)null);
            string sTmax = GetInputValue(DA, 23, (string)null);
            string sTmin = GetInputValue(DA, 24, (string)null);
            string sNoise = GetInputValue(DA, 25, (string)null);
            string overrideSettings = GetInputValue(DA, 26, (string)null);
            bool overrideSettingsRestoreAfterwards = GetInputValue(DA, 27, true);
            string refinerCheckpoint = GetInputValue(DA, 28, (string)null);
            double refinerSwitchAt = GetInputValue(DA, 29, (double)0);
            bool disableExtraNetworks = GetInputValue(DA, 30, false);
            string comments = GetInputValue(DA, 31, (string)null);
            bool enableHr = GetInputValue(DA, 32, false);
            int firstPhaseWidth = GetInputValue(DA, 33, 0);
            int firstPhaseHeight = GetInputValue(DA, 34, 0);
            double hrScale = GetInputValue(DA, 35, (double)2.0);
            string hrUpscaler = GetInputValue(DA, 36, (string)null);
            int hrSecondPassSteps = GetInputValue(DA, 37, 0);
            int hrResizeX = GetInputValue(DA, 38, 0);
            int hrResizeY = GetInputValue(DA, 39, 0);
            string hrCheckpointName = GetInputValue(DA, 40, (string)null);
            string hrSamplerName = GetInputValue(DA, 41, (string)null);
            string hrPrompt = GetInputValue(DA, 42, "");
            string hrNegativePrompt = GetInputValue(DA, 43, "");
            string samplerIndex = GetInputValue(DA, 44, "Euler");
            string scriptName = GetInputValue(DA, 45, (string)null);
            List<string> scriptArgs = new List<string>();
            DA.GetDataList<string>(46, scriptArgs);
            bool sendImages = GetInputValue(DA, 47, true);
            bool saveImages = GetInputValue(DA, 48, false);
            string alwaysonScripts = GetInputValue(DA, 49, "{}");

            //AddRuntimeMessage(GH_RuntimeMessageLevel.Error, Params.Input.Count.ToString());

            // Construct the dictionary
            var parameters = new
            {
                prompt,
                negative_prompt = negPrompt,
                styles,
                seed,
                subseed,
                subseed_strength = subseedStrength,
                seed_resize_from_h = seedResizeFromH,
                seed_resize_from_w = seedResizeFromW,
                sampler_name = samplerName,
                batch_size = batchSize,
                n_iter = nIter,
                steps,
                cfg_scale = cfgScale,
                width,
                height,
                restore_faces = restoreFaces,
                tiling,
                do_not_save_samples = doNotSaveSamples,
                do_not_save_grid = doNotSaveGrid,
                eta,
                denoising_strength = denoisingStrength,
                s_min_uncond = sMinUncond,
                s_churn = sChurn,
                s_tmax = sTmax,
                s_tmin = sTmin,
                s_noise = sNoise,
                override_settings = overrideSettings,
                override_settings_restore_afterwards = overrideSettingsRestoreAfterwards,
                refiner_checkpoint = refinerCheckpoint,
                refiner_switch_at = refinerSwitchAt,
                disable_extra_networks = disableExtraNetworks,
                comments,
                enable_hr = enableHr,
                firstphase_width = firstPhaseWidth,
                firstphase_height = firstPhaseHeight,
                hr_scale = hrScale,
                hr_upscaler = hrUpscaler,
                hr_second_pass_steps = hrSecondPassSteps,
                hr_resize_x = hrResizeX,
                hr_resize_y = hrResizeY,
                hr_checkpoint_name = hrCheckpointName,
                hr_sampler_name = hrSamplerName,
                hr_prompt = hrPrompt,
                hr_negative_prompt = hrNegativePrompt,
                sampler_index = samplerIndex,
                script_name = scriptName,
                script_args = scriptArgs,
                send_images = sendImages,
                save_images = saveImages,
                alwayson_scripts = Newtonsoft.Json.JsonConvert.DeserializeObject(alwaysonScripts)
            };

            string dataOut = Newtonsoft.Json.JsonConvert.SerializeObject(parameters);

            DA.SetData(0, dataOut);
        }


        /// <summary>
        /// Provides an Icon for the component.
        /// </summary>
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                //You can add image files to your project resources and access them like this:
                // return Resources.IconForThisComponent;
                return Properties.Resources.SDGenerateOptions_Full;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("06AED067-59A4-40C8-859E-D577E777D11E"); }
        }
    }
}