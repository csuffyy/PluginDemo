#pragma once

using namespace System;
using namespace PluginContract;

[Plugin("C++�˷�", "1.1.1.1")]
ref class CppPlugin : public IPlugin
{
public:
	CppPlugin();

public:
	virtual property String^ Name
	{
		String^ get() { return "C++�˷����"; };
		//void set(String^ s) sealed { };
	}

	virtual int __clrcall Invoke(int arg1, int arg2)
	{
		return arg1 * arg2;
	}
};

