﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Semgus.Model;
using Semgus.Model.Smt;
using Semgus.Model.Smt.Terms;

namespace Semgus.Parser.Json
{
    internal class ChcEvent : ParseEvent
    {
        private readonly SemgusChc _chc;

        public SmtIdentifier? Id => _chc.Id;
        public SemgusChc.SemanticRelation Head => _chc.Head;

        public IReadOnlyList<SemgusChc.SemanticRelation> BodyRelations => _chc.BodyRelations;

        public IReadOnlyCollection<SmtIdentifier>? InputVariables => _chc.InputVariables?.Select(v => v.Name).ToList();
        public IReadOnlyCollection<SmtIdentifier>? OutputVariables => _chc.OutputVariables?.Select(v => v.Name).ToList();
        public IReadOnlyCollection<SmtIdentifier>? Variables => _chc.VariableBindings.Select(b => b.Id).ToList();
        public SmtTerm Constraint => _chc.Constraint;
        public ConstructorModel? Constructor { get; }

        public SemgusChc.SymbolTable Symbols => _chc.Symbols;

        public ChcEvent(SemgusChc chc) : base("chc", "semgus")
        {
            _chc = chc;
            if (_chc.Binder.Constructor != null)
            {
                Constructor = new(_chc.Binder.Constructor.Name,
                                  _chc.Binder.Bindings.OrderBy(b => b.Index).Select(b => b.Binding.Id),
                                  _chc.Binder.Constructor.Children.Select(s => s.Name),
                                  _chc.Binder.ParentType.Name);
            }
        }

        public record ConstructorModel(SmtIdentifier Name, IEnumerable<SmtIdentifier> Arguments, IEnumerable<SmtSortIdentifier> ArgumentSorts, SmtSortIdentifier ReturnSort);
    }
}
