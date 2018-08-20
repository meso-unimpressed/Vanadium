﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chromium.Event;
using HtmlTexture.DX11.Nodes;
using VVVV.HtmlTexture.DX11.Core;
using VVVV.PluginInterfaces.V2;

namespace VVVV.HtmlTexture.DX11.Nodes
{
    [PluginInfo(
        Name = "EventTunneler",
        Category = "HtmlTexture.Operation",
        Author = "MESO, microdee",
        Tags = "onloaded, oncreated",
        Help = "Notify events upstream"
    )]
    public class EventsTunnelingNode : PersistentOperationNode<EmptyOperation>
    {
        [Output("On Loaded", IsBang = true)]
        public ISpread<int> FLoaded;
        [Output("On Created", IsBang = true)]
        public ISpread<int> FCreated;

        protected override int SliceCount()
        {
            return 1;
        }

        protected override EmptyOperation CreateOps(int i)
        {
            var ops = base.CreateOps(i);

            void OpsOnOnBeforeExecute(HtmlTextureOperation operation, HtmlTextureWrapper wrapper)
            {
                FLoaded[i] += wrapper.LoadedFrame ? 1 : 0;
                FCreated[i] += wrapper.CreatedFrame ? 1 : 0;
                //operation.OnBeforeExecute -= OpsOnOnBeforeExecute;
            }
            ops.OnBeforeExecute += OpsOnOnBeforeExecute;
            return ops;
        }

        protected override void PreEvaluate(int sprmax)
        {
            base.PreEvaluate(sprmax);
            FLoaded.SliceCount = FCreated.SliceCount = sprmax;
        }

        protected override void UpdateOps(ref EmptyOperation ops, int i)
        {
            ops.ExecuteOnLoad = true;
        }
    }
}