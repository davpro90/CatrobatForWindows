#include "pch.h"
#include "Script.h"

#include <windows.system.threading.h>
#include <windows.foundation.h>
#include <ppltasks.h>

using namespace Windows::System::Threading;
using namespace Windows::Foundation;

Script::Script(TypeOfScript scriptType, string spriteReference, Object *parent) :
	m_scriptType(scriptType), m_spriteReference(spriteReference), m_parent(parent)
{
	m_brickList = new list<Brick*>();
}

void Script::addBrick(Brick *brick)
{
	m_brickList->push_back(brick);
}

void Script::addSpriteReference(string spriteReference)
{
	m_spriteReference = spriteReference;
}

Script::TypeOfScript Script::getType()
{
	return m_scriptType;
}

string Script::SpriteReference()
{
	return m_spriteReference;
}

int Script::BrickListSize()
{
	return m_brickList->size();
}

Brick *Script::GetBrick(int index)
{
	list<Brick*>::iterator it = m_brickList->begin();
	advance(it, index);
	return *it;
}

void Script::Execute()
{
	int x = m_scriptType;
	auto WorkItem = ref new WorkItemHandler(
		[this](IAsyncAction^ workItem)
	{
		for (int i = 0; i < BrickListSize(); i++)
		{
			GetBrick(i)->Execute();
		}
		
		Concurrency::wait(10);
	});

	IAsyncAction^ ThreadPoolWorkItem = ThreadPool::RunAsync(WorkItem);
}

Object *Script::Parent()
{
	return m_parent;
}