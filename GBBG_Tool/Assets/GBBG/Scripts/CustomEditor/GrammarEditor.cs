using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

namespace GBBG
{
	public class GrammarEditor : EditorWindow
	{
		public Grammar grammar;
		public enum RuleType { Scope, Split, Repeat, Grid, Component, Margin, PostProcess}

		public enum RuleMode { ProductionRules, PostProcessRules}
		public RuleMode ruleMode = RuleMode.ProductionRules;

		//toolbar
		const string grammarAssetsFolder = "GBBG_Assets";
		const string ruleFolderPath = "Assets/GBBG_Assets/";
		int difIndex = 0;

		//rule selection
		int selectedRuleIndex;
		Vector2 ruleSelectorScroll;
		string ruleSelectorFilter = "";

		//rule inspector
		int ruleInspectorWidth;

		bool inspectorFoldoutTracker1;
		bool inspectorFoldoutTracker2;
		bool inspectorFoldoutTracker3;

		[MenuItem("Window/GBBG/Grammar Editor")]
		public static void ShowWindow()
		{
			GetWindow(typeof(GrammarEditor), false, "Grammar Editor");
		}

		private void OnDisable()
		{
			EditorUtility.SetDirty(grammar);
			AssetDatabase.SaveAssetIfDirty(grammar);
		}

		private void OnGUI()
		{
			minSize = new Vector2(550, 200);

			DrawGrammarSeletor();
			DrawToolbar();
			GUILayout.Space(10);
			if (grammar != null)
			{
				GUILayout.BeginHorizontal();
					GUILayout.BeginVertical("Box", GUILayout.Width(position.width / 2));
						DrawRuleSelector();
					GUILayout.EndVertical();
					GUILayout.Space(5);
					ruleInspectorWidth = (int)(position.width / 2) - 25;
					EditorGUILayout.BeginVertical("Box", GUILayout.Width(ruleInspectorWidth), GUILayout.MinWidth(200), GUILayout.Height(position.height - 60));
						DrawRuleInspector();
					EditorGUILayout.EndVertical();
				GUILayout.EndHorizontal();
			}
		}

		#region Toolbar
		private void DrawToolbar()
		{
			float buttonWidth = position.width / 6 + 5;
			EditorGUI.BeginDisabledGroup(grammar == null);
			GUILayout.BeginHorizontal(EditorStyles.toolbar);
			if (GUILayout.Button("New Rule", GUILayout.Width(buttonWidth)))
			{
				if (ruleMode == RuleMode.ProductionRules)
					NewRuleWindow.ShowWindow(new Vector2(Screen.width / 2, Screen.height / 2), this);
				else
					CreatePostProcess();
			}
			bool disabled = true;
			if(grammar != null)
			{
				if (ruleMode == RuleMode.ProductionRules)
					disabled = grammar.rules.Count == 0;
				else
					disabled = grammar.postProcesses.Count == 0;
			}
			EditorGUI.BeginDisabledGroup (disabled);
			if (GUILayout.Button("Delete Rule", GUILayout.Width(buttonWidth)))
			{
				if (ruleMode == RuleMode.ProductionRules)
					DeleteRuleWindow.ShowWindow(this, new Vector2(Screen.width / 2, Screen.height / 2));
				else
					DeleteSelectedPostProcess();
			}
			EditorGUI.EndDisabledGroup();
			if (GUILayout.Button("Duplicate Rule", GUILayout.Width(buttonWidth)))
			{
				if (ruleMode == RuleMode.ProductionRules)
					DuplicateRule(grammar.rules[selectedRuleIndex]);
				else
					CreatePostProcess(grammar.postProcesses[selectedRuleIndex]);
			}
			if(GUILayout.Button("Create Shape", GUILayout.Width(buttonWidth)))
			{
				CreateShapeWindow.ShowWindow(new Vector2(Screen.width / 2, Screen.height / 2), this);
			}
			RuleMode prev = ruleMode;
			ruleMode = (RuleMode)EditorGUILayout.EnumPopup(ruleMode);
			if (ruleMode != prev)
				selectedRuleIndex = 0;

			GUILayout.EndHorizontal();
			EditorGUI.EndDisabledGroup();
		}


		private void DeleteSelectedPostProcess()
		{
			grammar.postProcesses.RemoveAt(selectedRuleIndex);
		}

		private void CreatePostProcess()
		{
			grammar.postProcesses.Add(new PostProcess("New Post-Process"));
			selectedRuleIndex = grammar.postProcesses.Count - 1;
			EditorUtility.SetDirty(grammar);
			AssetDatabase.SaveAssetIfDirty(grammar);
		}

		private void CreatePostProcess(PostProcess process)
		{
			grammar.postProcesses.Add(new PostProcess(process));
			selectedRuleIndex = grammar.postProcesses.Count - 1;
			EditorUtility.SetDirty(grammar);
			AssetDatabase.SaveAssetIfDirty(grammar);
		}

		private void CreateNewRule<T>(string name) where T : Rule
		{
			ScriptableObject newRule = ScriptableObject.CreateInstance(typeof(T));
			newRule.name = name;

			SaveRule(newRule);
			((T)newRule).Init();
		}

		private void DuplicateRule(Rule rule)
		{
			ScriptableObject newRule = Instantiate(rule);
			SaveRule(newRule);
		}

		private void SaveRule(ScriptableObject newRule)
		{
			string path = GetRuleSavePath();

			//check for assets with the same name
			string[] GUIDs = AssetDatabase.FindAssets("t:Rule " + newRule.name, new string[] { path });
			if (GUIDs.Length == 0)
				AssetDatabase.CreateAsset(newRule, path + "/" + newRule.name + ".asset");
			else
			{
				//add an identifier to modify the name slightly
				difIndex = 1;
				while (true)
				{
					GUIDs = AssetDatabase.FindAssets("t:Rule " + newRule.name + difIndex.ToString(), new string[] { path });
					if (GUIDs.Length == 0)
					{
						AssetDatabase.CreateAsset(newRule, path + "/" + newRule.name + difIndex.ToString() + ".asset");
						break;
					}
					else
						difIndex++;
				}
			}

			grammar.rules.Add((Rule)newRule);
			selectedRuleIndex = grammar.rules.Count - 1;
			EditorUtility.SetDirty(grammar);
			AssetDatabase.SaveAssets();
		}

		private string GetRuleSavePath()
		{
			//find or create path
			if (!AssetDatabase.IsValidFolder($"Assets/{grammarAssetsFolder}"))
				AssetDatabase.CreateFolder("Assets", grammarAssetsFolder);
			string grammarPath = ruleFolderPath + grammar.name;
			if (!AssetDatabase.IsValidFolder(grammarPath))
				AssetDatabase.CreateFolder($"Assets/{grammarAssetsFolder}", grammar.name);
			string rulePath = grammarPath + "/Rules";
			if (!AssetDatabase.IsValidFolder(rulePath))
				AssetDatabase.CreateFolder(grammarPath, "Rules");
			
			return rulePath;
		}

		private string GetShapeSavePath()
		{
			//find or create path
			if (!AssetDatabase.IsValidFolder($"Assets/{grammarAssetsFolder}"))
				AssetDatabase.CreateFolder("Assets", grammarAssetsFolder);
			string grammarPath = ruleFolderPath + grammar.name;
			if (!AssetDatabase.IsValidFolder(grammarPath))
				AssetDatabase.CreateFolder($"Assets/{grammarAssetsFolder}", grammar.name);
			string shapePath = grammarPath + "/Shapes";
			if (!AssetDatabase.IsValidFolder(shapePath))
				AssetDatabase.CreateFolder(grammarPath, "Shapes");

			return shapePath;
		}

		public class NewRuleWindow : EditorWindow
		{
			static GrammarEditor grammarEditor;
			public static void ShowWindow(Vector2 position, GrammarEditor _grammarEditor)
			{
				grammarEditor = _grammarEditor;
				NewRuleWindow window = ScriptableObject.CreateInstance<NewRuleWindow>();
				window.position = new Rect(position.x, position.y, 260, 150);
				window.titleContent = new GUIContent("Select rule to create");
				window.ShowUtility();
			}

			private void OnGUI()
			{
				Vector2 buttonSize = new Vector2(121, 47);
				EditorGUILayout.BeginHorizontal();
				if(GUILayout.Button(new GUIContent("Scope"), GUILayout.Width(buttonSize.x), GUILayout.Height(buttonSize.y)))
				{
					grammarEditor.CreateNewRule<RuleScope>("New Scope Rule");
					this.Close();
				}
				if (GUILayout.Button(new GUIContent("Split"), GUILayout.Width(buttonSize.x), GUILayout.Height(buttonSize.y)))
				{
					grammarEditor.CreateNewRule<RuleSplit>("New Split Rule");
					this.Close();
				}
				EditorGUILayout.EndHorizontal();
				EditorGUILayout.BeginHorizontal();
				if (GUILayout.Button(new GUIContent("Repeat"), GUILayout.Width(buttonSize.x), GUILayout.Height(buttonSize.y)))
				{
					grammarEditor.CreateNewRule<RuleRepeat>("New Repeat Rule");
					this.Close();
				}
				if (GUILayout.Button(new GUIContent("Grid"), GUILayout.Width(buttonSize.x), GUILayout.Height(buttonSize.y)))
				{
					grammarEditor.CreateNewRule<RuleGrid>("New Grid Rule");
					this.Close();
				}
				EditorGUILayout.EndHorizontal();
				EditorGUILayout.BeginHorizontal();
				if (GUILayout.Button(new GUIContent("Component"), GUILayout.Width(buttonSize.x), GUILayout.Height(buttonSize.y)))
				{
					grammarEditor.CreateNewRule<RuleComponent>("New Component Rule");
					this.Close();
				}
				if (GUILayout.Button(new GUIContent("Margin"), GUILayout.Width(buttonSize.x), GUILayout.Height(buttonSize.y)))
				{
					grammarEditor.CreateNewRule<RuleCorner>("New Margin Rule");
					this.Close();
				}
				EditorGUILayout.EndHorizontal();

			}

			private void OnLostFocus()
			{
				Close();
			}
		}

		private void DeleteSelectedRule()
		{
			ScriptableObject ruleToDelete = grammar.rules[selectedRuleIndex];
			grammar.rules.RemoveAt(selectedRuleIndex);
			
			AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(ruleToDelete));
			AssetDatabase.Refresh();
			selectedRuleIndex = Mathf.Clamp(selectedRuleIndex, 0, grammar.rules.Count-1);
			Repaint();
		}

		public class DeleteRuleWindow : EditorWindow
		{
			static GrammarEditor editor;
			public static void ShowWindow(GrammarEditor grammarEditor, Vector2 position)
			{
				editor = grammarEditor;
				DeleteRuleWindow window = ScriptableObject.CreateInstance<DeleteRuleWindow>();
				window.position = new Rect(position.x, position.y, 280, 150);
				window.titleContent = new GUIContent("Are you sure?");
				window.ShowUtility();
			}

			private void OnGUI()
			{
				GUILayout.Space(10);
				GUILayout.Label("This will permanently delete the selected rule.\nContinue?");
				GUILayout.Space(10);
				GUILayout.BeginHorizontal();
				if (GUILayout.Button("YES"))
				{
					editor.DeleteSelectedRule();
					Close();
				}
				if (GUILayout.Button("NO"))
				{
					Close();
				}
				GUILayout.EndHorizontal();

			}

			private void OnLostFocus()
			{
				Close();
			}
		}

		public class CreateShapeWindow : EditorWindow
		{

			static GrammarEditor grammarEditor;
			string symbol;
			public static void ShowWindow(Vector2 position, GrammarEditor _grammarEditor)
			{
				grammarEditor = _grammarEditor;
				CreateShapeWindow window = ScriptableObject.CreateInstance<CreateShapeWindow>();
				window.position = new Rect(position.x, position.y, 260, 200);
				window.titleContent = new GUIContent("Select shape to create");
				window.ShowUtility();
			}

			private void OnEnable()
			{
				symbol = "";
			}

			private void OnGUI()
			{
				symbol = EditorGUILayout.TextField(new GUIContent("Name", "The name and symbol of the shape that will be created"),symbol);
				Vector2 buttonSize = new Vector2(121, 30);
				if (GUILayout.Button(new GUIContent("Empty Shape"), GUILayout.Height(buttonSize.y)))
				{
					string path = EditorUtilities.CopyPrefab(CreateShapeEditor.EmptyShapeAssetPath, symbol == "" ? "New Empty Shape" : $"{symbol} (Empty)", grammarEditor.GetShapeSavePath());
					Shape newShape = (Shape)AssetDatabase.LoadAssetAtPath(path, typeof(Shape));
					newShape.Symbol = symbol;
					this.Close();
				}
				if (GUILayout.Button(new GUIContent("2D Shape"), GUILayout.Height(buttonSize.y)))
				{
					string path = EditorUtilities.CopyPrefab(CreateShapeEditor.Shape2DAssetPath, symbol == "" ? "New 2D Shape" : $"{symbol} (2D)", grammarEditor.GetShapeSavePath());
					Shape newShape = (Shape)AssetDatabase.LoadAssetAtPath(path, typeof(Shape));
					newShape.Symbol = symbol;
					this.Close();
				}
				if (GUILayout.Button(new GUIContent("3D Shape"), GUILayout.Height(buttonSize.y)))
				{
					string path = EditorUtilities.CopyPrefab(CreateShapeEditor.Shape3DAssetPath, symbol == "" ? "New 3D Shape" : $"{symbol} (3D)", grammarEditor.GetShapeSavePath());
					Shape newShape = (Shape)AssetDatabase.LoadAssetAtPath(path, typeof(Shape));
					newShape.Symbol = symbol;
					this.Close();
				}
				EditorUtilities.DrawInfo("Shapes are saved in " + grammarEditor.GetShapeSavePath());
			}

			private void OnLostFocus()
			{
				Close();
			}
		}
		#endregion

		#region Inspector
		private void DrawRuleInspector()
		{
			GUILayout.Label("Rule Inspector", EditorStyles.boldLabel);
			GUILayout.Space(5);

			if (ruleMode == RuleMode.ProductionRules)
			{
				if (grammar.rules.Count == 0)
					return;

				Rule rule = grammar.rules[selectedRuleIndex];
				if (rule.GetType() == typeof(RuleRepeat))
				{
					DrawRuleRepeatInspector((RuleRepeat)rule);
				}
				else if (rule.GetType() == typeof(RuleScope))
				{
					DrawRuleScopeInspector((RuleScope)rule);
				}
				else if (rule.GetType() == typeof(RuleSplit))
				{
					DrawRuleSplitInspector((RuleSplit)rule);
				}
				else if (rule.GetType() == typeof(RuleGrid))
				{
					DrawRuleGridInspector((RuleGrid)rule);
				}
				else if (rule.GetType() == typeof(RuleComponent))
				{
					DrawRuleComponentInspector((RuleComponent)rule);
				}
				else if (rule.GetType() == typeof(RuleCorner))
				{
					DrawRuleCornerInspector((RuleCorner)rule);
				}
				EditorUtility.SetDirty(rule);
				
			}
			else
			{
				if (grammar.postProcesses.Count == 0)
					return;

				PostProcess process = grammar.postProcesses[selectedRuleIndex];
				DrawPostProcessInspector(process);
			}

		}

		private void DrawPostProcessInspector(PostProcess process)
		{
			EditorGUI.BeginDisabledGroup(true);
			EditorGUILayout.TextField("Rule Type", "PostProcess");
			EditorGUI.EndDisabledGroup();

			process.symbol = EditorGUILayout.TextField(new GUIContent("Symbol", "The symbol (string) that identifies the predecessor terminal shape."), process.symbol);
			if (!grammar.IsVocabulary(process.symbol))
				EditorGUILayout.HelpBox("Shape not found in this grammar.", MessageType.Warning);
			else if (!grammar.IsTerminal(process.symbol))
				EditorGUILayout.HelpBox("Shape is not terminal.", MessageType.Warning);

			process.asset = (GameObject)EditorGUILayout.ObjectField(new GUIContent("Final Asset"), process.asset, typeof(GameObject), false);

		}

		private void DrawRuleCornerInspector(RuleCorner rule)
		{
			//rule type
			EditorGUI.BeginDisabledGroup(true);
			EditorGUILayout.TextField("Rule Type", "Margin");
			EditorGUI.EndDisabledGroup();

			//rule name
			string name = EditorGUILayout.DelayedTextField("Name", rule.name);
			if (name != rule.name)
				rule.RenameRule(name);

			//predecessor
			rule.predescesor = EditorGUILayout.TextField(new GUIContent("Predecessor", "The symbol (string) that identifies the predecessor shape."), rule.predescesor);
			if (!grammar.IsVocabulary(rule.predescesor))
				EditorGUILayout.HelpBox("Predecessor not found in this grammar.", MessageType.Warning);

			//axis
			rule.axis = (Axis)EditorGUILayout.EnumPopup(new GUIContent("Axis"), rule.axis);

			//margin
			rule.marginType = (RuleCorner.MarginType)EditorGUILayout.EnumPopup(new GUIContent("Margin Type"), rule.marginType);
			if (rule.marginType == RuleCorner.MarginType.Absolute)
				rule.margin = Mathf.Max(0, EditorGUILayout.FloatField(new GUIContent("Margin"), rule.margin));
			else
				rule.margin = EditorGUILayout.Slider(new GUIContent("Margin"), rule.margin, 0, 0.5f);

			//corner orientation
			rule.cornerOrientation = (RuleCorner.CornerOrientation)EditorGUILayout.EnumPopup(new GUIContent("Corner Orientation"), rule.cornerOrientation);

			//includecenterpiece
			rule.includeCenterPiece = EditorGUILayout.Toggle(new GUIContent("Include Center Piece"), rule.includeCenterPiece);

			//successor
			List<Successor> succesors = rule.succesor; 
			List<GUIContent> labels;
			if (rule.includeCenterPiece)
			{
				labels = new List<GUIContent>()
				{
					new GUIContent("Corner"),
					new GUIContent("Edge"),
					new GUIContent("Center")
				};
				succesors.Resize(labels.Count);
				succesors = EditorUtilities.DrawLabeledSuccessorList(succesors, "Successors", ref inspectorFoldoutTracker1, labels);
				rule.succesor[0] = succesors[0];
				rule.succesor[1] = succesors[1];
				rule.succesor[2] = succesors[2];

			}
			else
			{
				labels = new List<GUIContent>()
				{
					new GUIContent("Corner"),
					new GUIContent("Edge"),
				};
				succesors.Resize(labels.Count);
				succesors = EditorUtilities.DrawLabeledSuccessorList(succesors, "Successors", ref inspectorFoldoutTracker1, labels);
				rule.succesor[0] = succesors[0];
				rule.succesor[1] = succesors[1];
			}
		}

		private void DrawRuleComponentInspector(RuleComponent rule)
		{
			//rule type
			EditorGUI.BeginDisabledGroup(true);
			EditorGUILayout.TextField("Rule Type", "Component");
			EditorGUI.EndDisabledGroup();

			//rule name
			string name = EditorGUILayout.DelayedTextField("Name", rule.name);
			if (name != rule.name)
				rule.RenameRule(name);

			//predecessor
			rule.predescesor = EditorGUILayout.TextField(new GUIContent("Predecessor", "The symbol (string) that identifies the predecessor shape."), rule.predescesor);
			if (!grammar.IsVocabulary(rule.predescesor))
				EditorGUILayout.HelpBox("Predecessor not found in this grammar.", MessageType.Warning);

			//axis
			rule.axis = (Axis)EditorGUILayout.EnumPopup(new GUIContent("Axis", "Top and Bottom are the planes perpendicular to the axis"), rule.axis);

			//split mode
			rule.splitMode = (RuleComponent.SplitMode)EditorGUILayout.EnumPopup(new GUIContent("Split Mode", "Determines which components are the ones to be saved"), rule.splitMode);

			//successors
			List<Successor> succesors = new List<Successor>();
			
			List<GUIContent> labels;
			switch (rule.splitMode)
			{
				case RuleComponent.SplitMode.AllFaces:
					succesors.Resize(3);
					succesors[0] = rule.succesor[0];
					succesors[1] = rule.succesor[1];
					succesors[2] = rule.succesor[2];
					labels = new List<GUIContent>{
						new GUIContent("Sides"),
						new GUIContent("Top"),
						new GUIContent("Bottom")
					};
					succesors = EditorUtilities.DrawLabeledSuccessorList(succesors, "Successors", ref inspectorFoldoutTracker1, labels);
					//succesors = EditorUtilities.DrawObjectList(succesors, "Successors", ref inspectorFoldoutTracker1, labels, true);
					rule.succesor[0] = succesors[0];
					rule.succesor[1] = succesors[1];
					rule.succesor[2] = succesors[2];
					break;
				case RuleComponent.SplitMode.Sides:
					succesors.Resize(1, null);
					succesors[0] = rule.succesor[0];
					labels = new List<GUIContent>{
						new GUIContent("Sides")
					};
					succesors = EditorUtilities.DrawLabeledSuccessorList(succesors, "Successors", ref inspectorFoldoutTracker1, labels);
					rule.succesor[0] = succesors[0];
					break;
				case RuleComponent.SplitMode.SidesPlusTop:
					succesors.Resize(2);
					succesors[0] = rule.succesor[0];
					succesors[1] = rule.succesor[1];
					labels = new List<GUIContent>{
						new GUIContent("Sides"),
						new GUIContent("Top")
					};
					succesors = EditorUtilities.DrawLabeledSuccessorList(succesors, "Successors", ref inspectorFoldoutTracker1, labels);
					rule.succesor[0] = succesors[0];
					rule.succesor[1] = succesors[1];
					break;
				case RuleComponent.SplitMode.Top:
					succesors.Resize(1);
					succesors[0] = rule.succesor[1];
					labels = new List<GUIContent>{
						new GUIContent("Top"),
					};
					succesors = EditorUtilities.DrawLabeledSuccessorList(succesors, "Successors", ref inspectorFoldoutTracker1, labels);
					rule.succesor[1] = succesors[0];
					break;
				case RuleComponent.SplitMode.Bottom:
					succesors.Resize(1, null);
					succesors[0] = rule.succesor[2];
					labels = new List<GUIContent>{
						new GUIContent("Bottom")
					};
					succesors = EditorUtilities.DrawLabeledSuccessorList(succesors, "Successors", ref inspectorFoldoutTracker1, labels);
					rule.succesor[2] = succesors[0];
					break;
				case RuleComponent.SplitMode.SidesPlusBottom:
					succesors.Resize(2);
					succesors[0] = rule.succesor[0];
					succesors[1] = rule.succesor[2];
					labels = new List<GUIContent>{
						new GUIContent("Sides"),
						new GUIContent("Bottom")
					};
					succesors = EditorUtilities.DrawLabeledSuccessorList(succesors, "Successors", ref inspectorFoldoutTracker1, labels);
					rule.succesor[0] = succesors[0];
					rule.succesor[2] = succesors[1];
					break;
				case RuleComponent.SplitMode.TopPlusBottom:
					succesors.Resize(2);
					succesors[0] = rule.succesor[1];
					succesors[1] = rule.succesor[2];
					labels = new List<GUIContent>{
						new GUIContent("Top"),
						new GUIContent("Bottom")
					};
					succesors = EditorUtilities.DrawLabeledSuccessorList(succesors, "Successors", ref inspectorFoldoutTracker1, labels);
					rule.succesor[1] = succesors[0];
					rule.succesor[2] = succesors[1];
					break;
				default:
					succesors.Resize(3);
					succesors[0] = rule.succesor[0];
					succesors[1] = rule.succesor[1];
					succesors[2] = rule.succesor[2];
					labels = new List<GUIContent>{
						new GUIContent("Sides"),
						new GUIContent("Top"),
						new GUIContent("Bottom")
					};
					succesors = EditorUtilities.DrawLabeledSuccessorList(succesors, "Successors", ref inspectorFoldoutTracker1, labels);
					rule.succesor[0] = succesors[0];
					rule.succesor[1] = succesors[1];
					rule.succesor[2] = succesors[2];
					break;
			}

		}

		private void DrawRuleGridInspector(RuleGrid rule)
		{
			//rule type
			EditorGUI.BeginDisabledGroup(true);
			EditorGUILayout.TextField("Rule Type", "Grid");
			EditorGUI.EndDisabledGroup();

			//rule name
			string name = EditorGUILayout.DelayedTextField("Name", rule.name);
			if (name != rule.name)
				rule.RenameRule(name);

			//predecessor
			rule.predescesor = EditorGUILayout.TextField(new GUIContent("Predecessor", "The symbol (string) that identifies the predecessor shape."), rule.predescesor);
			if (!grammar.IsVocabulary(rule.predescesor))
				EditorGUILayout.HelpBox("Predecessor not found in this grammar.", MessageType.Warning);

			//axis
			rule.axis = (Axis)EditorGUILayout.EnumPopup(new GUIContent("Axis", "The axis parallel to the division planes."), rule.axis);

			//successor
			EditorGUILayout.LabelField("Successor");
			rule.succesor[0] = EditorUtilities.DrawSuccessorField(rule.succesor[0]);
			//rule.succesor[0] = (Successor)EditorGUILayout.ObjectField(new GUIContent("Successor"), rule.succesor[0], typeof(Successor), false);

		}

		private void DrawRuleSplitInspector(RuleSplit rule)
		{
			//rule type
			EditorGUI.BeginDisabledGroup(true);
			EditorGUILayout.TextField("Rule Type", "Split");
			EditorGUI.EndDisabledGroup();

			//rule name
			string name = EditorGUILayout.DelayedTextField("Name", rule.name);
			if (name != rule.name)
				rule.RenameRule(name);

			//predecessor
			rule.predescesor = EditorGUILayout.TextField(new GUIContent("Predecessor", "The symbol (string) that identifies the predecessor shape."), rule.predescesor);
			if (!grammar.IsVocabulary(rule.predescesor))
				EditorGUILayout.HelpBox("Predecessor not found in this grammar.", MessageType.Warning);

			//axis
			rule.axis = (Axis)EditorGUILayout.EnumPopup(new GUIContent("Axis", "The axis perpendicular to the division planes."), rule.axis);

			//split direction
			rule.cuttingType = (RuleSplit.CuttingType)EditorGUILayout.EnumPopup(new GUIContent("Cut direction"), rule.cuttingType);

			//successors
			rule.succesor = EditorUtilities.DrawSuccessorList(rule.succesor, new GUIContent("Successor"), ref inspectorFoldoutTracker1);
			
			//splitpoints
			int splitPointsCount = Mathf.Max(0, rule.succesor.Count - 1);
			rule.splitPoints.Resize(splitPointsCount);
			rule.splitPoints = EditorUtilities.DrawFloatList(rule.splitPoints, "Split Points", ref inspectorFoldoutTracker2, true);

		}

		private void DrawRuleScopeInspector(RuleScope rule)
		{
			//rule type
			EditorGUI.BeginDisabledGroup(true);
			EditorGUILayout.TextField("Rule Type", "Scope");
			EditorGUI.EndDisabledGroup();

			//rule name
			string name = EditorGUILayout.DelayedTextField("Name", rule.name);
			if (name != rule.name)
				rule.RenameRule(name);

			//predecessor
			rule.predescesor = EditorGUILayout.TextField(new GUIContent("Predecessor", "The symbol (string) that identifies the predecessor shape."), rule.predescesor);
			if (!grammar.IsVocabulary(rule.predescesor))
				EditorGUILayout.HelpBox("Predecessor not found in this grammar.", MessageType.Warning);

			//succesor
			EditorGUILayout.LabelField("Successor");
			rule.succesor[0] = EditorUtilities.DrawSuccessorField(rule.succesor[0]);
			//rule.succesor[0] = (Successor)EditorGUILayout.ObjectField(new GUIContent("Successor"), rule.succesor[0], typeof(Successor), false);

			//translation
			rule.applyTranslation = EditorGUILayout.BeginToggleGroup(new GUIContent("Translation"), rule.applyTranslation);
			//value
			rule.translation = EditorGUILayout.Vector3Field(new GUIContent("Translation Value"), rule.translation);
			//add/set
			rule.translationMode = (RuleScope.Mode)EditorGUILayout.EnumPopup(new GUIContent("Mode"), rule.translationMode);
			//world/object space
			EditorGUI.BeginDisabledGroup(rule.translationMode != RuleScope.Mode.Add);
			rule.translationSpace = (Space)EditorGUILayout.EnumPopup(new GUIContent("Space"), rule.translationSpace);
			EditorGUI.EndDisabledGroup();
			EditorGUILayout.EndToggleGroup();

			//rotation
			rule.applyRotation = EditorGUILayout.BeginToggleGroup(new GUIContent("Rotation"), rule.applyRotation);
			//value
			rule.rotation = EditorGUILayout.Vector3Field(new GUIContent("Rotation Euler"), rule.rotation);
			//world/object space
			rule.rotationSpace = (Space)EditorGUILayout.EnumPopup(new GUIContent("Space"), rule.rotationSpace);
			//add/set
			rule.rotationMode = (RuleScope.Mode)EditorGUILayout.EnumPopup(new GUIContent("Mode"), rule.rotationMode);
			EditorGUILayout.EndToggleGroup();

			//scale
			rule.applyScale = EditorGUILayout.BeginToggleGroup(new GUIContent("Scale"), rule.applyScale);
			//value
			rule.scale = EditorGUILayout.Vector3Field(new GUIContent("Value"), rule.scale);
			//add/set
			rule.scaleMode = (RuleScope.Mode)EditorGUILayout.EnumPopup(new GUIContent("Mode"), rule.scaleMode);
			//absolute/relative
			rule.scaleAbsolute = (RuleScope.Mode2)EditorGUILayout.EnumPopup(new GUIContent("Absolute"), rule.scaleAbsolute);
			EditorGUILayout.EndToggleGroup();

		}

		private void DrawRuleRepeatInspector(RuleRepeat rule)
		{
			//rule type
			EditorGUI.BeginDisabledGroup(true);
			EditorGUILayout.TextField("Rule Type", "Repeat");
			EditorGUI.EndDisabledGroup();

			//rule name
			string name = EditorGUILayout.DelayedTextField("Name", rule.name);
			if (name != rule.name)
				rule.RenameRule(name);

			//predecessor
			rule.predescesor = EditorGUILayout.TextField(new GUIContent("Predecessor", "The symbol (string) that identifies the predecessor shape."), rule.predescesor);
			if (!grammar.IsVocabulary(rule.predescesor))
				EditorGUILayout.HelpBox("Predecessor not found in this grammar.", MessageType.Warning);

			//axis
			rule.axis = (Axis)EditorGUILayout.EnumPopup(new GUIContent("Axis", "The axis perpendicular to the division planes. Ignored if predecessor is 2D."), rule.axis);

			//successor
			EditorGUILayout.LabelField("Successor");
			rule.succesor[0] = EditorUtilities.DrawSuccessorField(rule.succesor[0]);
			//rule.succesor[0] = (Successor)EditorGUILayout.ObjectField(new GUIContent("Successor"), rule.succesor[0], typeof(Successor), false);
		}

		#endregion

		#region RuleSelector
		private void DrawRuleSelector()
		{
			GUILayout.BeginHorizontal();
			GUILayout.Label("Rule Selector", EditorStyles.boldLabel);
			ruleSelectorFilter = GUILayout.TextField(ruleSelectorFilter, EditorStyles.toolbarSearchField, GUILayout.Width(200));
			GUILayout.EndHorizontal();
			GUILayout.Space(5);

			List<string> ruleNames = new List<string>();
			switch (ruleMode)
			{
				case RuleMode.ProductionRules:
					ruleNames = GetProductionRuleNames();
					break;
				case RuleMode.PostProcessRules:
					ruleNames = GetPostProcessNames();
					break;
				default:
					break;
			}

			int selectionGridColumns = (int)position.width / 2 / 120;
			int selectionGridHeight = Mathf.CeilToInt(ruleNames.Count / (float)selectionGridColumns);
			ruleSelectorScroll = GUILayout.BeginScrollView(ruleSelectorScroll, false, true);
			int index = selectedRuleIndex;
			selectedRuleIndex = GUILayout.SelectionGrid(selectedRuleIndex, ruleNames.ToArray(), selectionGridColumns, GUILayout.Height(selectionGridHeight * 50), GUILayout.Width(position.width / 2 - 28));
			if(index != selectedRuleIndex)
			{
				AssetDatabase.SaveAssetIfDirty(grammar.rules[selectedRuleIndex]);
			}
			GUILayout.EndScrollView();
		}

		private List<string> GetProductionRuleNames()
		{
			List<string> ruleNames = new List<string>();
			int ruleCount = grammar.rules.Count;
			for (int i = 0; i < ruleCount; i++)
			{
				if (ruleSelectorFilter == "" || grammar.rules[i].name.ToLower().Contains(ruleSelectorFilter.ToLower()))
					ruleNames.Add(grammar.rules[i].name);
			}
			return ruleNames;
		}

		private List<string> GetPostProcessNames()
		{
			List<string> ruleNames = new List<string>();
			int ruleCount = grammar.postProcesses.Count;
			for (int i = 0; i < ruleCount; i++)
			{
				if (ruleSelectorFilter == "" || grammar.postProcesses[i].symbol.ToLower().Contains(ruleSelectorFilter.ToLower()))
					ruleNames.Add(grammar.postProcesses[i].symbol);
			}
			return ruleNames;
		}

		#endregion

		#region Grammar selector
		private void DrawGrammarSeletor()
		{
			GUILayout.BeginHorizontal();
			GUILayout.Label("Grammar selection", GUILayout.Width(115));
			Grammar g = grammar;
			grammar = (Grammar)EditorGUILayout.ObjectField(grammar, typeof(Grammar), false);
			if (g != grammar)
				selectedRuleIndex = 0;
			GUILayout.EndHorizontal();
			GUILayout.Space(5);
		}
		#endregion

	}
}


