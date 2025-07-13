
using System;

[AttributeUsage(AttributeTargets.All)]
internal class Impl : Attribute{
	public Type? IF { get; }
	public Type[]? Types{get;set;}
	public Impl(Type? IF = null) {
		this.IF = IF;
	}
	public Impl(Type[]?  Types){
		this.Types = Types;
	}
}
