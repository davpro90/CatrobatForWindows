#pragma once

#include "Brick.h"

class HideBrick :
	public Brick
{
public:
	HideBrick(std::shared_ptr<Script> parent);
	void Execute();
};