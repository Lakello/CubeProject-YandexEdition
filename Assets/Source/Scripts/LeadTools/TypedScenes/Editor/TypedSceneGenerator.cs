#if UNITY_EDITOR
using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.IO;
using System.Reflection;

namespace LeadTools.TypedScenes.Editor
{
	public static class TypedSceneGenerator
	{
		public static string Generate(AnalyzableScene scene)
		{
			var sceneName = scene.Name;
			var targetUnit = new CodeCompileUnit();
			var targetNamespace = new CodeNamespace(GeneratorSettings.Namespace);
			var targetClass = new CodeTypeDeclaration(sceneName);
			targetNamespace.Imports.Add(new CodeNamespaceImport(GeneratorSettings.UnityEngineImport));
			targetNamespace.Imports.Add(new CodeNamespaceImport(GeneratorSettings.SceneManagementImport));
			targetNamespace.Imports.Add(new CodeNamespaceImport(GeneratorSettings.FSMImport));
			targetNamespace.Imports.Add(new CodeNamespaceImport(GeneratorSettings.GameFSMImport));
			targetNamespace.Imports.Add(new CodeNamespaceImport(GeneratorSettings.TypedScenesCoreImport));

			targetClass.BaseTypes.Add(
				new CodeTypeReference(
					GeneratorSettings.BaseTypeName,
					new CodeTypeReference(GeneratorSettings.GameStateMachine)));

			targetClass.TypeAttributes = TypeAttributes.Class | TypeAttributes.Public;

			AddConstantValue(targetClass, typeof(string), GeneratorSettings.SceneNameField, sceneName);

			var methodFactory = new TypedSceneMethodFactory();

			methodFactory.AddLoadingMethod(targetClass, false, false, true);
			methodFactory.AddLoadingMethod(targetClass, true, false, true);
			methodFactory.AddLoadingMethod(targetClass, false, true, true);
			methodFactory.AddLoadingMethod(targetClass, true, true, true);
			methodFactory.AddLoadingMethod(targetClass, false, true, false);
			methodFactory.AddLoadingMethod(targetClass, true, true, false);

			targetNamespace.Types.Add(targetClass);
			targetUnit.Namespaces.Add(targetNamespace);

			var provider = CodeDomProvider.CreateProvider(GeneratorSettings.DomProvider);

			var options = new CodeGeneratorOptions
			{
				BracingStyle = GeneratorSettings.BracingStyle
			};

			var code = new StringWriter();
			provider.GenerateCodeFromCompileUnit(targetUnit, code, options);

			return code.ToString();
		}

		private static void AddConstantValue(CodeTypeDeclaration targetClass, Type type, string name, string value)
		{
			var pathConstant = new CodeMemberField(type, name)
			{
				Attributes = MemberAttributes.Private | MemberAttributes.Const,
				InitExpression = new CodePrimitiveExpression(value)
			};

			targetClass.Members.Add(pathConstant);
		}
	}
}
#endif