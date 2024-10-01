using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GBBG
{
	[System.Serializable]
	public class PostProcess
	{
		public string symbol;
		public GameObject asset;

		public PostProcess() { }
		public PostProcess(string symbol) { this.symbol = symbol; }
		public PostProcess(string symbol, GameObject asset) {  this.asset = asset; this.symbol = symbol; }
		public PostProcess(PostProcess postProcess) { symbol = postProcess.symbol; asset = postProcess.asset; }
	}

}
