using System.CodeDom;
using System.Collections.Generic;
using LeadTools.FSM.GameFSM;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace LeadTools.TypedScenes.Editor
{
	public class TypedSceneMethodFactory
	{
		private CodeMemberMethod _loadMethod;
		private List<string> _statementArguments;

		public void AddLoadingMethod(
			CodeTypeDeclaration targetClass,
			bool isAsyncLoad,
			bool isStateLoad,
			bool isArgumentLoad)
		{
			var statementTypeParameters = InitMethod(isAsyncLoad);

			TryAddStateLoad(isStateLoad, statementTypeParameters);

			TryAddArgumentLoad(isArgumentLoad, statementTypeParameters);

			TryAddAsyncLoad(isAsyncLoad);

			var loadingStatement = GetLoadingStatement(isAsyncLoad, _statementArguments, statementTypeParameters);

			var loadingModeParameter = new CodeParameterDeclarationExpression(
				nameof(LoadSceneMode),
				MethodFactorySettings.LoadSceneModeParameter);

			_loadMethod.Parameters.Add(loadingModeParameter);
			_loadMethod.Statements.Add(new CodeSnippetExpression(loadingStatement));
			targetClass.Members.Add(_loadMethod);
		}

		private List<string> InitMethod(bool isAsyncLoad)
		{
			_loadMethod = new CodeMemberMethod
			{
				Name = MethodFactorySettings.GetLoadName(isAsyncLoad),
				Attributes = MemberAttributes.Public | MemberAttributes.Static
			};

			_statementArguments = new List<string>();
			var statementTypeParameters = new List<string>();

			return statementTypeParameters;
		}

		private void TryAddAsyncLoad(bool isAsyncLoad)
		{
			if (isAsyncLoad)
			{
				_loadMethod.ReturnType = new CodeTypeReference(nameof(AsyncOperation));
			}
		}

		private void TryAddArgumentLoad(bool isArgumentLoad, List<string> statementTypeParameters)
		{
			if (isArgumentLoad)
			{
				_loadMethod.TypeParameters.Add(new CodeTypeParameter(MethodFactorySettings.ArgumentT));

				statementTypeParameters.Add(MethodFactorySettings.ArgumentT);

				AddParameter(MethodFactorySettings.ArgumentT, MethodFactorySettings.Argument);
			}
		}

		private void TryAddStateLoad(bool isStateLoad, List<string> statementTypeParameters)
		{
			if (isStateLoad)
			{
				AddParameter(nameof(GameStateMachine), MethodFactorySettings.Machine);

				var targetTypeParameter = new CodeTypeParameter(MethodFactorySettings.StateT);

				statementTypeParameters.Add(MethodFactorySettings.StateT);

				var tState = new CodeTypeReference(MethodFactorySettings.StateType);
				tState.TypeArguments.Add(new CodeTypeReference(nameof(GameStateMachine)));

				targetTypeParameter.Constraints.Add(tState);

				_loadMethod.TypeParameters.Add(targetTypeParameter);
			}
		}

		private void AddParameter(string type, string argumentName)
		{
			var parameter = new CodeParameterDeclarationExpression(type, argumentName);
			_loadMethod.Parameters.Add(parameter);
			_statementArguments.Add(argumentName);
		}

		private string GetLoadingStatement(bool isAsyncLoad, List<string> statementArguments, List<string> statementTypeParameters)
		{
			string loadingStatement = string.Empty;

			var name = MethodFactorySettings.StatementName;
			var typeParameters = string.Empty;
			var arguments = MethodFactorySettings.GetStartParameters;

			for (int i = 0; i < statementTypeParameters.Count; i++)
			{
				var type = string.Empty;

				type += MethodFactorySettings.TryGetComma(i);

				type += statementTypeParameters[i];

				typeParameters += type;
			}

			foreach (var argument in statementArguments)
			{
				arguments += $", {argument}";
			}

			loadingStatement = $"{name}<{typeParameters}>{arguments})";

			loadingStatement = isAsyncLoad ? $"return {loadingStatement}" : loadingStatement;

			return loadingStatement;
		}
	}
}