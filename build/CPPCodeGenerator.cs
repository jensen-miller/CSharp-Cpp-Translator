/******************************************************************************
 *  Copyright (c) 2019 Jensen Miller
 *
 *  License: The GNU License
 *  
 *  This file is part of IoTDotNet.
 *
 *  IoTDotNet is free software: you can redistribute it and/or modify
 *  it under the terms of the GNU General Public License as published by
 *  the Free Software Foundation, either version 3 of the License, or
 *  (at your option) any later version.
 *
 *  IoTDotNet is distributed in the hope that it will be useful,
 *  but WITHOUT ANY WARRANTY; without even the implied warranty of
 *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *  GNU General Public License for more details.
 *
 *  You should have received a copy of the GNU General Public License
 *  along with IoTDotNet.  If not, see <https://www.gnu.org/licenses/>.
 *****************************************************************************/
using System;

using System.Collections.Generic;
using System.Text;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;



namespace CS_CPP_Translator
{
    internal class CPPCodeGenerator : CSharpSyntaxVisitor<StringBuilder>
    {
        public const string INCLUDE_DIRECTIVE = "#include";
        public const string DEFINE_DIRECTIVE = "#define";
        public const string NAMESPACE_KEYWORD = "namespace";
        public const string CLASS_KEYWORD = "class";
        public const string POINTER_SYM = "*";
        
        protected CPPCodeGenerator()
        {
            
        }

        public static StringBuilder GenerateCode(CSharpSyntaxNode syntaxNode)
        {
            CPPCodeGenerator newCodeGenerator = new CPPCodeGenerator();            
            return syntaxNode.Accept(newCodeGenerator);            
        }



        /// <summary>
        /// Entry of compilation unit.
        /// </summary>
        /// <param name="node"></param>
        public override StringBuilder VisitCompilationUnit(CompilationUnitSyntax node)
        {
            StringBuilder fileBuilder = new StringBuilder();
            foreach (UsingDirectiveSyntax usingDirSyn in node.Usings)
            {
                fileBuilder.Append(usingDirSyn.Accept(this));
            }
            fileBuilder.Append("\r\n");
            return fileBuilder.Append(node.Members.First().Accept(this));
        }



        /// <summary>
        /// Visit a using directive for importing libraries/packages.
        /// </summary>
        /// <param name="node"></param>
        public override StringBuilder VisitUsingDirective(UsingDirectiveSyntax node)
        {            
            return new StringBuilder().AppendFormat(
                "{0} <{1}.h>\r\n",
                INCLUDE_DIRECTIVE,
                node.Name.Accept(this)
                );
        }



        /// <summary>
        /// Idenfier Name resolve.
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public override StringBuilder VisitIdentifierName(IdentifierNameSyntax node)
        {
            return new StringBuilder().Append(node.Identifier.Value);
        }



        //
        //  Declarations
        //


        /// <summary>
        /// Namespace Declaration.
        /// </summary>
        /// <param name="node"></param>
        public override StringBuilder VisitNamespaceDeclaration(NamespaceDeclarationSyntax node)
        {
            return new StringBuilder().AppendFormat(
                "{0} {1}\r\n{{\r\n\t{2}\r\n}}\r\n",
                NAMESPACE_KEYWORD,
                node.Name.Accept(this),
                VisitEachMemberDeclarations(node.Members)
                );
        }



        /// <summary>
        /// Visit individual members
        /// </summary>
        /// <param name="nodes"></param>
        /// <returns></returns>
        private StringBuilder VisitEachMemberDeclarations(SyntaxList<MemberDeclarationSyntax> nodes)
        {
            StringBuilder membersBuilder = new StringBuilder();
            foreach (MemberDeclarationSyntax member in nodes)
            {
                membersBuilder.Append(member.Accept(this));
            }
            return membersBuilder;
        }



        /// <summary>
        /// Class Declaration.
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public override StringBuilder VisitClassDeclaration(ClassDeclarationSyntax node)
        {
            return new StringBuilder().AppendFormat(
                "{0} {1}\r\n{{\r\n{2}\r\n}};\r\n",
                CLASS_KEYWORD,
                node.Identifier.ValueText,
                VisitEachMemberDeclarations(node.Members)
                );
        }



        /// <summary>
        /// MethodDeclaration -> ReturnType Identifier ParameterList Body
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public override StringBuilder VisitMethodDeclaration(MethodDeclarationSyntax node)
        {                  
            return new StringBuilder().AppendFormat(
                "{0} {1} {2}({3})\r\n{{\r\n{4}\r\n}}\r\n", 
                ProcessMethodModifiers(node.Modifiers),
                node.ReturnType.Accept(this),
                node.Identifier.ValueText,
                node.ParameterList.Accept(this),
                node.Body.Accept(this)
                );
        }

        private StringBuilder ProcessMethodModifiers(SyntaxTokenList syntaxTokens)
        {
            StringBuilder modifierBuilder = new StringBuilder();
            foreach (var modifier in syntaxTokens)
            {
                switch (modifier.Kind())
                {
                    case SyntaxKind.PublicKeyword:
                        modifierBuilder.Insert(0, "public:\r\n");
                        break;
                    case SyntaxKind.PrivateKeyword:
                        modifierBuilder.Insert(0, "private:\r\n");
                        break;
                    case SyntaxKind.InternalKeyword:                                                
                    case SyntaxKind.ProtectedKeyword:
                        modifierBuilder.Insert(0, "protected:\r\n");
                        break;
                    case SyntaxKind.StaticKeyword:
                        modifierBuilder.Append("static");
                        break;
                    default:
                        break;
                }
            }
            return modifierBuilder;
        }



        /// <summary>
        /// PredefinedType -> Keyword
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public override StringBuilder VisitPredefinedType(PredefinedTypeSyntax node)
        {
            return new StringBuilder(node.Keyword.ValueText);
        }



        /// <summary>
        /// InvocationExpression -> MethodCall ArgumentList
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public override StringBuilder VisitInvocationExpression(InvocationExpressionSyntax node)
        {
            return new StringBuilder().AppendFormat(
                "{0}({1})",
                node.Expression.Accept(this),
                node.ArgumentList.Accept(this)
                );
        }


        /// <summary>
        /// ParameterList -> Parameter(s)
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public override StringBuilder VisitParameterList(ParameterListSyntax node)
        {
            StringBuilder parameterListBuilder = new StringBuilder();        
            if (node.Parameters.Any())
            {
                foreach (var parameter in node.Parameters)
                {
                    parameterListBuilder.Append(parameter.Accept(this));
                    parameterListBuilder.Append(',');
                }
                //  Remove trailing comma
                parameterListBuilder.Remove(parameterListBuilder.Length - 1, 1);
            }                   
            return parameterListBuilder;
        }



        /// <summary>
        /// Block -> Statement(s)
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public override StringBuilder VisitBlock(BlockSyntax node)
        {
            StringBuilder blockBuilder = new StringBuilder();
            foreach (var statement in node.Statements)
            {
                blockBuilder.Append(statement.Accept(this));
            }
            return blockBuilder;
        }



        /// <summary>
        /// Parameter -> Type Identifier
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public override StringBuilder VisitParameter(ParameterSyntax node)
        {
            return new StringBuilder().AppendFormat("{0} {1}",
                node.Type.Accept(this),
                node.Identifier.ValueText
                );
        }



        /// <summary>
        /// Statement -> Expression
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public override StringBuilder VisitExpressionStatement(ExpressionStatementSyntax node)
        {
            return new StringBuilder().AppendFormat("{0};",
                node.Expression.Accept(this));            
        }



        /// <summary>
        /// Modified Type -> Array Type
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public override StringBuilder VisitArrayType(ArrayTypeSyntax node)
        {
            return new StringBuilder().AppendFormat("{0} {1}", node.ElementType.Accept(this), POINTER_SYM);
        }




        /// <summary>
        /// MemberAccess -> Expression
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public override StringBuilder VisitMemberAccessExpression(MemberAccessExpressionSyntax node)
        {
            return new StringBuilder().AppendFormat("{0}::{1}",
                node.Expression.Accept(this),
                node.Name.Accept(this));
        }



        /// <summary>
        /// ArgumentList -> Argument(s)
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public override StringBuilder VisitArgumentList(ArgumentListSyntax node)
        {
            StringBuilder argumentBuilder = new StringBuilder();
            if (node.Arguments.Any())
            {
                foreach (var arg in node.Arguments)
                {
                    argumentBuilder.Append(arg.Accept(this));
                    argumentBuilder.Append(',');
                }
                //  Remove trailing comma
                argumentBuilder.Remove(argumentBuilder.Length - 1, 1);
            }
            return argumentBuilder;
        }



        /// <summary>
        /// Argument -> Expression
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public override StringBuilder VisitArgument(ArgumentSyntax node)
        {
            return new StringBuilder().Append(node.Expression.Accept(this));
        }



        /// <summary>
        /// Expression -> Literal
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public override StringBuilder VisitLiteralExpression(LiteralExpressionSyntax node)
        {            
            return new StringBuilder(node.Token.Text);
        }




        public override StringBuilder VisitAccessorDeclaration(AccessorDeclarationSyntax node)
        {
            return base.VisitAccessorDeclaration(node);
        }

        public override StringBuilder VisitAccessorList(AccessorListSyntax node)
        {
            return base.VisitAccessorList(node);
        }

        public override StringBuilder VisitAliasQualifiedName(AliasQualifiedNameSyntax node)
        {
            return base.VisitAliasQualifiedName(node);
        }

        public override StringBuilder VisitAnonymousMethodExpression(AnonymousMethodExpressionSyntax node)
        {
            return base.VisitAnonymousMethodExpression(node);
        }

        public override StringBuilder VisitAnonymousObjectCreationExpression(AnonymousObjectCreationExpressionSyntax node)
        {
            return base.VisitAnonymousObjectCreationExpression(node);
        }

        public override StringBuilder VisitAnonymousObjectMemberDeclarator(AnonymousObjectMemberDeclaratorSyntax node)
        {
            return base.VisitAnonymousObjectMemberDeclarator(node);
        }





        public override StringBuilder VisitArrayCreationExpression(ArrayCreationExpressionSyntax node)
        {
            return base.VisitArrayCreationExpression(node);
        }

        public override StringBuilder VisitArrayRankSpecifier(ArrayRankSpecifierSyntax node)
        {
            return base.VisitArrayRankSpecifier(node);
        }

        public override StringBuilder VisitArrowExpressionClause(ArrowExpressionClauseSyntax node)
        {
            return base.VisitArrowExpressionClause(node);
        }

        public override StringBuilder VisitAssignmentExpression(AssignmentExpressionSyntax node)
        {
            return base.VisitAssignmentExpression(node);
        }

        public override StringBuilder VisitAttribute(AttributeSyntax node)
        {
            return base.VisitAttribute(node);
        }

        public override StringBuilder VisitAttributeArgument(AttributeArgumentSyntax node)
        {
            return base.VisitAttributeArgument(node);
        }

        public override StringBuilder VisitAttributeArgumentList(AttributeArgumentListSyntax node)
        {
            return base.VisitAttributeArgumentList(node);
        }

        public override StringBuilder VisitAttributeList(AttributeListSyntax node)
        {
            return base.VisitAttributeList(node);
        }

        public override StringBuilder VisitAttributeTargetSpecifier(AttributeTargetSpecifierSyntax node)
        {
            return base.VisitAttributeTargetSpecifier(node);
        }

        public override StringBuilder VisitAwaitExpression(AwaitExpressionSyntax node)
        {
            return base.VisitAwaitExpression(node);
        }

        public override StringBuilder VisitBadDirectiveTrivia(BadDirectiveTriviaSyntax node)
        {
            return base.VisitBadDirectiveTrivia(node);
        }

        public override StringBuilder VisitBaseExpression(BaseExpressionSyntax node)
        {
            return base.VisitBaseExpression(node);
        }

        public override StringBuilder VisitBaseExpressionTypeClause(BaseExpressionTypeClauseSyntax node)
        {
            return base.VisitBaseExpressionTypeClause(node);
        }

        public override StringBuilder VisitBaseList(BaseListSyntax node)
        {
            return base.VisitBaseList(node);
        }

        public override StringBuilder VisitBinaryExpression(BinaryExpressionSyntax node)
        {
            return base.VisitBinaryExpression(node);
        }



        public override StringBuilder VisitBracketedArgumentList(BracketedArgumentListSyntax node)
        {
            return base.VisitBracketedArgumentList(node);
        }

        public override StringBuilder VisitBracketedParameterList(BracketedParameterListSyntax node)
        {
            return base.VisitBracketedParameterList(node);
        }

        public override StringBuilder VisitBreakStatement(BreakStatementSyntax node)
        {
            return base.VisitBreakStatement(node);
        }

        public override StringBuilder VisitCasePatternSwitchLabel(CasePatternSwitchLabelSyntax node)
        {
            return base.VisitCasePatternSwitchLabel(node);
        }

        public override StringBuilder VisitCaseSwitchLabel(CaseSwitchLabelSyntax node)
        {
            return base.VisitCaseSwitchLabel(node);
        }

        public override StringBuilder VisitCastExpression(CastExpressionSyntax node)
        {
            return base.VisitCastExpression(node);
        }

        public override StringBuilder VisitCatchClause(CatchClauseSyntax node)
        {
            return base.VisitCatchClause(node);
        }

        public override StringBuilder VisitCatchDeclaration(CatchDeclarationSyntax node)
        {
            return base.VisitCatchDeclaration(node);
        }

        public override StringBuilder VisitCatchFilterClause(CatchFilterClauseSyntax node)
        {
            return base.VisitCatchFilterClause(node);
        }

        public override StringBuilder VisitCheckedExpression(CheckedExpressionSyntax node)
        {
            return base.VisitCheckedExpression(node);
        }

        public override StringBuilder VisitCheckedStatement(CheckedStatementSyntax node)
        {
            return base.VisitCheckedStatement(node);
        }


        public override StringBuilder VisitClassOrStructConstraint(ClassOrStructConstraintSyntax node)
        {
            return base.VisitClassOrStructConstraint(node);
        }

        public override StringBuilder VisitConditionalAccessExpression(ConditionalAccessExpressionSyntax node)
        {
            return base.VisitConditionalAccessExpression(node);
        }

        public override StringBuilder VisitConditionalExpression(ConditionalExpressionSyntax node)
        {
            return base.VisitConditionalExpression(node);
        }

        public override StringBuilder VisitConstantPattern(ConstantPatternSyntax node)
        {
            return base.VisitConstantPattern(node);
        }

        public override StringBuilder VisitConstructorConstraint(ConstructorConstraintSyntax node)
        {
            return base.VisitConstructorConstraint(node);
        }

        public override StringBuilder VisitConstructorDeclaration(ConstructorDeclarationSyntax node)
        {
            return base.VisitConstructorDeclaration(node);
        }

        public override StringBuilder VisitConstructorInitializer(ConstructorInitializerSyntax node)
        {
            return base.VisitConstructorInitializer(node);
        }

        public override StringBuilder VisitContinueStatement(ContinueStatementSyntax node)
        {
            return base.VisitContinueStatement(node);
        }

        public override StringBuilder VisitConversionOperatorDeclaration(ConversionOperatorDeclarationSyntax node)
        {
            return base.VisitConversionOperatorDeclaration(node);
        }

        public override StringBuilder VisitConversionOperatorMemberCref(ConversionOperatorMemberCrefSyntax node)
        {
            return base.VisitConversionOperatorMemberCref(node);
        }

        public override StringBuilder VisitCrefBracketedParameterList(CrefBracketedParameterListSyntax node)
        {
            return base.VisitCrefBracketedParameterList(node);
        }

        public override StringBuilder VisitCrefParameter(CrefParameterSyntax node)
        {
            return base.VisitCrefParameter(node);
        }

        public override StringBuilder VisitCrefParameterList(CrefParameterListSyntax node)
        {
            return base.VisitCrefParameterList(node);
        }

        public override StringBuilder VisitDeclarationExpression(DeclarationExpressionSyntax node)
        {
            return base.VisitDeclarationExpression(node);
        }

        public override StringBuilder VisitDeclarationPattern(DeclarationPatternSyntax node)
        {
            return base.VisitDeclarationPattern(node);
        }

        public override StringBuilder VisitDefaultExpression(DefaultExpressionSyntax node)
        {
            return base.VisitDefaultExpression(node);
        }

        public override StringBuilder VisitDefaultSwitchLabel(DefaultSwitchLabelSyntax node)
        {
            return base.VisitDefaultSwitchLabel(node);
        }

        public override StringBuilder VisitDefineDirectiveTrivia(DefineDirectiveTriviaSyntax node)
        {
            return base.VisitDefineDirectiveTrivia(node);
        }

        public override StringBuilder VisitDelegateDeclaration(DelegateDeclarationSyntax node)
        {
            return base.VisitDelegateDeclaration(node);
        }

        public override StringBuilder VisitDestructorDeclaration(DestructorDeclarationSyntax node)
        {
            return base.VisitDestructorDeclaration(node);
        }

        public override StringBuilder VisitDiscardDesignation(DiscardDesignationSyntax node)
        {
            return base.VisitDiscardDesignation(node);
        }

        public override StringBuilder VisitDiscardPattern(DiscardPatternSyntax node)
        {
            return base.VisitDiscardPattern(node);
        }

        public override StringBuilder VisitDocumentationCommentTrivia(DocumentationCommentTriviaSyntax node)
        {
            return base.VisitDocumentationCommentTrivia(node);
        }

        public override StringBuilder VisitDoStatement(DoStatementSyntax node)
        {
            return base.VisitDoStatement(node);
        }

        public override StringBuilder VisitElementAccessExpression(ElementAccessExpressionSyntax node)
        {
            return base.VisitElementAccessExpression(node);
        }

        public override StringBuilder VisitElementBindingExpression(ElementBindingExpressionSyntax node)
        {
            return base.VisitElementBindingExpression(node);
        }

        public override StringBuilder VisitElifDirectiveTrivia(ElifDirectiveTriviaSyntax node)
        {
            return base.VisitElifDirectiveTrivia(node);
        }

        public override StringBuilder VisitElseClause(ElseClauseSyntax node)
        {
            return base.VisitElseClause(node);
        }

        public override StringBuilder VisitElseDirectiveTrivia(ElseDirectiveTriviaSyntax node)
        {
            return base.VisitElseDirectiveTrivia(node);
        }

        public override StringBuilder VisitEmptyStatement(EmptyStatementSyntax node)
        {
            return base.VisitEmptyStatement(node);
        }

        public override StringBuilder VisitEndIfDirectiveTrivia(EndIfDirectiveTriviaSyntax node)
        {
            return base.VisitEndIfDirectiveTrivia(node);
        }

        public override StringBuilder VisitEndRegionDirectiveTrivia(EndRegionDirectiveTriviaSyntax node)
        {
            return base.VisitEndRegionDirectiveTrivia(node);
        }

        public override StringBuilder VisitEnumDeclaration(EnumDeclarationSyntax node)
        {
            return base.VisitEnumDeclaration(node);
        }

        public override StringBuilder VisitEnumMemberDeclaration(EnumMemberDeclarationSyntax node)
        {
            return base.VisitEnumMemberDeclaration(node);
        }

        public override StringBuilder VisitEqualsValueClause(EqualsValueClauseSyntax node)
        {
            return base.VisitEqualsValueClause(node);
        }

        public override StringBuilder VisitErrorDirectiveTrivia(ErrorDirectiveTriviaSyntax node)
        {
            return base.VisitErrorDirectiveTrivia(node);
        }

        public override StringBuilder VisitEventDeclaration(EventDeclarationSyntax node)
        {
            return base.VisitEventDeclaration(node);
        }

        public override StringBuilder VisitEventFieldDeclaration(EventFieldDeclarationSyntax node)
        {
            return base.VisitEventFieldDeclaration(node);
        }

        public override StringBuilder VisitExplicitInterfaceSpecifier(ExplicitInterfaceSpecifierSyntax node)
        {
            return base.VisitExplicitInterfaceSpecifier(node);
        }

        public override StringBuilder VisitExternAliasDirective(ExternAliasDirectiveSyntax node)
        {
            return base.VisitExternAliasDirective(node);
        }

        public override StringBuilder VisitFieldDeclaration(FieldDeclarationSyntax node)
        {
            return base.VisitFieldDeclaration(node);
        }

        public override StringBuilder VisitFinallyClause(FinallyClauseSyntax node)
        {
            return base.VisitFinallyClause(node);
        }

        public override StringBuilder VisitFixedStatement(FixedStatementSyntax node)
        {
            return base.VisitFixedStatement(node);
        }

        public override StringBuilder VisitForEachStatement(ForEachStatementSyntax node)
        {
            return base.VisitForEachStatement(node);
        }

        public override StringBuilder VisitForEachVariableStatement(ForEachVariableStatementSyntax node)
        {
            return base.VisitForEachVariableStatement(node);
        }

        public override StringBuilder VisitForStatement(ForStatementSyntax node)
        {
            return base.VisitForStatement(node);
        }

        public override StringBuilder VisitFromClause(FromClauseSyntax node)
        {
            return base.VisitFromClause(node);
        }

        public override StringBuilder VisitGenericName(GenericNameSyntax node)
        {
            return base.VisitGenericName(node);
        }

        public override StringBuilder VisitGlobalStatement(GlobalStatementSyntax node)
        {
            return base.VisitGlobalStatement(node);
        }

        public override StringBuilder VisitGotoStatement(GotoStatementSyntax node)
        {
            return base.VisitGotoStatement(node);
        }

        public override StringBuilder VisitGroupClause(GroupClauseSyntax node)
        {
            return base.VisitGroupClause(node);
        }



        public override StringBuilder VisitIfDirectiveTrivia(IfDirectiveTriviaSyntax node)
        {
            return base.VisitIfDirectiveTrivia(node);
        }

        public override StringBuilder VisitIfStatement(IfStatementSyntax node)
        {
            return base.VisitIfStatement(node);
        }

        public override StringBuilder VisitImplicitArrayCreationExpression(ImplicitArrayCreationExpressionSyntax node)
        {
            return base.VisitImplicitArrayCreationExpression(node);
        }

        public override StringBuilder VisitImplicitElementAccess(ImplicitElementAccessSyntax node)
        {
            return base.VisitImplicitElementAccess(node);
        }

        public override StringBuilder VisitImplicitStackAllocArrayCreationExpression(ImplicitStackAllocArrayCreationExpressionSyntax node)
        {
            return base.VisitImplicitStackAllocArrayCreationExpression(node);
        }

        public override StringBuilder VisitIncompleteMember(IncompleteMemberSyntax node)
        {
            return base.VisitIncompleteMember(node);
        }

        public override StringBuilder VisitIndexerDeclaration(IndexerDeclarationSyntax node)
        {
            return base.VisitIndexerDeclaration(node);
        }

        public override StringBuilder VisitIndexerMemberCref(IndexerMemberCrefSyntax node)
        {
            return base.VisitIndexerMemberCref(node);
        }

        public override StringBuilder VisitInitializerExpression(InitializerExpressionSyntax node)
        {
            return base.VisitInitializerExpression(node);
        }

        public override StringBuilder VisitInterfaceDeclaration(InterfaceDeclarationSyntax node)
        {
            return base.VisitInterfaceDeclaration(node);
        }

        public override StringBuilder VisitInterpolatedStringExpression(InterpolatedStringExpressionSyntax node)
        {
            return base.VisitInterpolatedStringExpression(node);
        }

        public override StringBuilder VisitInterpolatedStringText(InterpolatedStringTextSyntax node)
        {
            return base.VisitInterpolatedStringText(node);
        }

        public override StringBuilder VisitInterpolation(InterpolationSyntax node)
        {
            return base.VisitInterpolation(node);
        }

        public override StringBuilder VisitInterpolationAlignmentClause(InterpolationAlignmentClauseSyntax node)
        {
            return base.VisitInterpolationAlignmentClause(node);
        }

        public override StringBuilder VisitInterpolationFormatClause(InterpolationFormatClauseSyntax node)
        {
            return base.VisitInterpolationFormatClause(node);
        }

        public override StringBuilder VisitIsPatternExpression(IsPatternExpressionSyntax node)
        {
            return base.VisitIsPatternExpression(node);
        }

        public override StringBuilder VisitJoinClause(JoinClauseSyntax node)
        {
            return base.VisitJoinClause(node);
        }

        public override StringBuilder VisitJoinIntoClause(JoinIntoClauseSyntax node)
        {
            return base.VisitJoinIntoClause(node);
        }

        public override StringBuilder VisitLabeledStatement(LabeledStatementSyntax node)
        {
            return base.VisitLabeledStatement(node);
        }

        public override StringBuilder VisitLetClause(LetClauseSyntax node)
        {
            return base.VisitLetClause(node);
        }

        public override StringBuilder VisitLineDirectiveTrivia(LineDirectiveTriviaSyntax node)
        {
            return base.VisitLineDirectiveTrivia(node);
        }



        public override StringBuilder VisitLoadDirectiveTrivia(LoadDirectiveTriviaSyntax node)
        {
            return base.VisitLoadDirectiveTrivia(node);
        }

        public override StringBuilder VisitLocalDeclarationStatement(LocalDeclarationStatementSyntax node)
        {
            return base.VisitLocalDeclarationStatement(node);
        }

        public override StringBuilder VisitLocalFunctionStatement(LocalFunctionStatementSyntax node)
        {
            return base.VisitLocalFunctionStatement(node);
        }

        public override StringBuilder VisitLockStatement(LockStatementSyntax node)
        {
            return base.VisitLockStatement(node);
        }

        public override StringBuilder VisitMakeRefExpression(MakeRefExpressionSyntax node)
        {
            return base.VisitMakeRefExpression(node);
        }

        public override StringBuilder VisitMemberBindingExpression(MemberBindingExpressionSyntax node)
        {
            return base.VisitMemberBindingExpression(node);
        }



        public override StringBuilder VisitNameColon(NameColonSyntax node)
        {
            return base.VisitNameColon(node);
        }

        public override StringBuilder VisitNameEquals(NameEqualsSyntax node)
        {
            return base.VisitNameEquals(node);
        }

        public override StringBuilder VisitNameMemberCref(NameMemberCrefSyntax node)
        {
            return base.VisitNameMemberCref(node);
        }

        public override StringBuilder VisitNullableDirectiveTrivia(NullableDirectiveTriviaSyntax node)
        {
            return base.VisitNullableDirectiveTrivia(node);
        }

        public override StringBuilder VisitNullableType(NullableTypeSyntax node)
        {
            return base.VisitNullableType(node);
        }

        public override StringBuilder VisitObjectCreationExpression(ObjectCreationExpressionSyntax node)
        {
            return base.VisitObjectCreationExpression(node);
        }

        public override StringBuilder VisitOmittedArraySizeExpression(OmittedArraySizeExpressionSyntax node)
        {
            return base.VisitOmittedArraySizeExpression(node);
        }

        public override StringBuilder VisitOmittedTypeArgument(OmittedTypeArgumentSyntax node)
        {
            return base.VisitOmittedTypeArgument(node);
        }

        public override StringBuilder VisitOperatorDeclaration(OperatorDeclarationSyntax node)
        {
            return base.VisitOperatorDeclaration(node);
        }

        public override StringBuilder VisitOperatorMemberCref(OperatorMemberCrefSyntax node)
        {
            return base.VisitOperatorMemberCref(node);
        }

        public override StringBuilder VisitOrderByClause(OrderByClauseSyntax node)
        {
            return base.VisitOrderByClause(node);
        }

        public override StringBuilder VisitOrdering(OrderingSyntax node)
        {
            return base.VisitOrdering(node);
        }




        public override StringBuilder VisitParenthesizedExpression(ParenthesizedExpressionSyntax node)
        {
            return base.VisitParenthesizedExpression(node);
        }

        public override StringBuilder VisitParenthesizedLambdaExpression(ParenthesizedLambdaExpressionSyntax node)
        {
            return base.VisitParenthesizedLambdaExpression(node);
        }

        public override StringBuilder VisitParenthesizedVariableDesignation(ParenthesizedVariableDesignationSyntax node)
        {
            return base.VisitParenthesizedVariableDesignation(node);
        }

        public override StringBuilder VisitPointerType(PointerTypeSyntax node)
        {
            return base.VisitPointerType(node);
        }

        public override StringBuilder VisitPositionalPatternClause(PositionalPatternClauseSyntax node)
        {
            return base.VisitPositionalPatternClause(node);
        }

        public override StringBuilder VisitPostfixUnaryExpression(PostfixUnaryExpressionSyntax node)
        {
            return base.VisitPostfixUnaryExpression(node);
        }

        public override StringBuilder VisitPragmaChecksumDirectiveTrivia(PragmaChecksumDirectiveTriviaSyntax node)
        {
            return base.VisitPragmaChecksumDirectiveTrivia(node);
        }

        public override StringBuilder VisitPragmaWarningDirectiveTrivia(PragmaWarningDirectiveTriviaSyntax node)
        {
            return base.VisitPragmaWarningDirectiveTrivia(node);
        }

        public override StringBuilder VisitPrefixUnaryExpression(PrefixUnaryExpressionSyntax node)
        {
            return base.VisitPrefixUnaryExpression(node);
        }

        public override StringBuilder VisitPropertyDeclaration(PropertyDeclarationSyntax node)
        {
            return base.VisitPropertyDeclaration(node);
        }

        public override StringBuilder VisitPropertyPatternClause(PropertyPatternClauseSyntax node)
        {
            return base.VisitPropertyPatternClause(node);
        }

        public override StringBuilder VisitQualifiedCref(QualifiedCrefSyntax node)
        {
            return base.VisitQualifiedCref(node);
        }

        public override StringBuilder VisitQualifiedName(QualifiedNameSyntax node)
        {
            return base.VisitQualifiedName(node);
        }

        public override StringBuilder VisitQueryBody(QueryBodySyntax node)
        {
            return base.VisitQueryBody(node);
        }

        public override StringBuilder VisitQueryContinuation(QueryContinuationSyntax node)
        {
            return base.VisitQueryContinuation(node);
        }

        public override StringBuilder VisitQueryExpression(QueryExpressionSyntax node)
        {
            return base.VisitQueryExpression(node);
        }

        public override StringBuilder VisitRangeExpression(RangeExpressionSyntax node)
        {
            return base.VisitRangeExpression(node);
        }

        public override StringBuilder VisitRecursivePattern(RecursivePatternSyntax node)
        {
            return base.VisitRecursivePattern(node);
        }

        public override StringBuilder VisitReferenceDirectiveTrivia(ReferenceDirectiveTriviaSyntax node)
        {
            return base.VisitReferenceDirectiveTrivia(node);
        }

        public override StringBuilder VisitRefExpression(RefExpressionSyntax node)
        {
            return base.VisitRefExpression(node);
        }

        public override StringBuilder VisitRefType(RefTypeSyntax node)
        {
            return base.VisitRefType(node);
        }

        public override StringBuilder VisitRefTypeExpression(RefTypeExpressionSyntax node)
        {
            return base.VisitRefTypeExpression(node);
        }

        public override StringBuilder VisitRefValueExpression(RefValueExpressionSyntax node)
        {
            return base.VisitRefValueExpression(node);
        }

        public override StringBuilder VisitRegionDirectiveTrivia(RegionDirectiveTriviaSyntax node)
        {
            return base.VisitRegionDirectiveTrivia(node);
        }

        public override StringBuilder VisitReturnStatement(ReturnStatementSyntax node)
        {
            return base.VisitReturnStatement(node);
        }

        public override StringBuilder VisitSelectClause(SelectClauseSyntax node)
        {
            return base.VisitSelectClause(node);
        }

        public override StringBuilder VisitShebangDirectiveTrivia(ShebangDirectiveTriviaSyntax node)
        {
            return base.VisitShebangDirectiveTrivia(node);
        }

        public override StringBuilder VisitSimpleBaseType(SimpleBaseTypeSyntax node)
        {
            return base.VisitSimpleBaseType(node);
        }

        public override StringBuilder VisitSimpleLambdaExpression(SimpleLambdaExpressionSyntax node)
        {
            return base.VisitSimpleLambdaExpression(node);
        }

        public override StringBuilder VisitSingleVariableDesignation(SingleVariableDesignationSyntax node)
        {
            return base.VisitSingleVariableDesignation(node);
        }

        public override StringBuilder VisitSizeOfExpression(SizeOfExpressionSyntax node)
        {
            return base.VisitSizeOfExpression(node);
        }

        public override StringBuilder VisitSkippedTokensTrivia(SkippedTokensTriviaSyntax node)
        {
            return base.VisitSkippedTokensTrivia(node);
        }

        public override StringBuilder VisitStackAllocArrayCreationExpression(StackAllocArrayCreationExpressionSyntax node)
        {
            return base.VisitStackAllocArrayCreationExpression(node);
        }

        public override StringBuilder VisitStructDeclaration(StructDeclarationSyntax node)
        {
            return base.VisitStructDeclaration(node);
        }

        public override StringBuilder VisitSubpattern(SubpatternSyntax node)
        {
            return base.VisitSubpattern(node);
        }

        public override StringBuilder VisitSwitchExpression(SwitchExpressionSyntax node)
        {
            return base.VisitSwitchExpression(node);
        }

        public override StringBuilder VisitSwitchExpressionArm(SwitchExpressionArmSyntax node)
        {
            return base.VisitSwitchExpressionArm(node);
        }

        public override StringBuilder VisitSwitchSection(SwitchSectionSyntax node)
        {
            return base.VisitSwitchSection(node);
        }

        public override StringBuilder VisitSwitchStatement(SwitchStatementSyntax node)
        {
            return base.VisitSwitchStatement(node);
        }

        public override StringBuilder VisitThisExpression(ThisExpressionSyntax node)
        {
            return base.VisitThisExpression(node);
        }

        public override StringBuilder VisitThrowExpression(ThrowExpressionSyntax node)
        {
            return base.VisitThrowExpression(node);
        }

        public override StringBuilder VisitThrowStatement(ThrowStatementSyntax node)
        {
            return base.VisitThrowStatement(node);
        }

        public override StringBuilder VisitTryStatement(TryStatementSyntax node)
        {
            return base.VisitTryStatement(node);
        }

        public override StringBuilder VisitTupleElement(TupleElementSyntax node)
        {
            return base.VisitTupleElement(node);
        }

        public override StringBuilder VisitTupleExpression(TupleExpressionSyntax node)
        {
            return base.VisitTupleExpression(node);
        }

        public override StringBuilder VisitTupleType(TupleTypeSyntax node)
        {
            return base.VisitTupleType(node);
        }

        public override StringBuilder VisitTypeArgumentList(TypeArgumentListSyntax node)
        {
            return base.VisitTypeArgumentList(node);
        }

        public override StringBuilder VisitTypeConstraint(TypeConstraintSyntax node)
        {
            return base.VisitTypeConstraint(node);
        }

        public override StringBuilder VisitTypeCref(TypeCrefSyntax node)
        {
            return base.VisitTypeCref(node);
        }

        public override StringBuilder VisitTypeOfExpression(TypeOfExpressionSyntax node)
        {
            return base.VisitTypeOfExpression(node);
        }

        public override StringBuilder VisitTypeParameter(TypeParameterSyntax node)
        {
            return base.VisitTypeParameter(node);
        }

        public override StringBuilder VisitTypeParameterConstraintClause(TypeParameterConstraintClauseSyntax node)
        {
            return base.VisitTypeParameterConstraintClause(node);
        }

        public override StringBuilder VisitTypeParameterList(TypeParameterListSyntax node)
        {
            return base.VisitTypeParameterList(node);
        }

        public override StringBuilder VisitUndefDirectiveTrivia(UndefDirectiveTriviaSyntax node)
        {
            return base.VisitUndefDirectiveTrivia(node);
        }

        public override StringBuilder VisitUnsafeStatement(UnsafeStatementSyntax node)
        {
            return base.VisitUnsafeStatement(node);
        }

        public override StringBuilder VisitUsingStatement(UsingStatementSyntax node)
        {
            return base.VisitUsingStatement(node);
        }

        public override StringBuilder VisitVariableDeclaration(VariableDeclarationSyntax node)
        {
            return base.VisitVariableDeclaration(node);
        }

        public override StringBuilder VisitVariableDeclarator(VariableDeclaratorSyntax node)
        {
            return base.VisitVariableDeclarator(node);
        }

        public override StringBuilder VisitVarPattern(VarPatternSyntax node)
        {
            return base.VisitVarPattern(node);
        }

        public override StringBuilder VisitWarningDirectiveTrivia(WarningDirectiveTriviaSyntax node)
        {
            return base.VisitWarningDirectiveTrivia(node);
        }

        public override StringBuilder VisitWhenClause(WhenClauseSyntax node)
        {
            return base.VisitWhenClause(node);
        }

        public override StringBuilder VisitWhereClause(WhereClauseSyntax node)
        {
            return base.VisitWhereClause(node);
        }

        public override StringBuilder VisitWhileStatement(WhileStatementSyntax node)
        {
            return base.VisitWhileStatement(node);
        }

        public override StringBuilder VisitXmlCDataSection(XmlCDataSectionSyntax node)
        {
            return base.VisitXmlCDataSection(node);
        }

        public override StringBuilder VisitXmlComment(XmlCommentSyntax node)
        {
            return base.VisitXmlComment(node);
        }

        public override StringBuilder VisitXmlCrefAttribute(XmlCrefAttributeSyntax node)
        {
            return base.VisitXmlCrefAttribute(node);
        }

        public override StringBuilder VisitXmlElement(XmlElementSyntax node)
        {
            return base.VisitXmlElement(node);
        }

        public override StringBuilder VisitXmlElementEndTag(XmlElementEndTagSyntax node)
        {
            return base.VisitXmlElementEndTag(node);
        }

        public override StringBuilder VisitXmlElementStartTag(XmlElementStartTagSyntax node)
        {
            return base.VisitXmlElementStartTag(node);
        }

        public override StringBuilder VisitXmlEmptyElement(XmlEmptyElementSyntax node)
        {
            return base.VisitXmlEmptyElement(node);
        }

        public override StringBuilder VisitXmlName(XmlNameSyntax node)
        {
            return base.VisitXmlName(node);
        }

        public override StringBuilder VisitXmlNameAttribute(XmlNameAttributeSyntax node)
        {
            return base.VisitXmlNameAttribute(node);
        }

        public override StringBuilder VisitXmlPrefix(XmlPrefixSyntax node)
        {
            return base.VisitXmlPrefix(node);
        }

        public override StringBuilder VisitXmlProcessingInstruction(XmlProcessingInstructionSyntax node)
        {
            return base.VisitXmlProcessingInstruction(node);
        }

        public override StringBuilder VisitXmlText(XmlTextSyntax node)
        {
            return base.VisitXmlText(node);
        }

        public override StringBuilder VisitXmlTextAttribute(XmlTextAttributeSyntax node)
        {
            return base.VisitXmlTextAttribute(node);
        }

        public override StringBuilder VisitYieldStatement(YieldStatementSyntax node)
        {
            return base.VisitYieldStatement(node);
        }
    }
}
