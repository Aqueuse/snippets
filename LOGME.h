#pragma once

#include "CoreMinimal.h"

#define UELOG(s, ...)			UE_LOG(LogEngine, Log,		TEXT(s), ##__VA_ARGS__)
#define P_WARNING(s, ...)		UE_LOG(LogEngine, Warning,	TEXT(s), ##__VA_ARGS__)
#define P_ERROR(s, ...)			UE_LOG(LogEngine, Error,		TEXT(s), ##__VA_ARGS__)

#define P_SCREEN_LOG(s)			{ if(GEngine) {GEngine->AddOnScreenDebugMessage(-1, 2.0f, FColor::Green, s); }}
#define P_SCREEN_WARNING(s)		{ if(GEngine) {GEngine->AddOnScreenDebugMessage(-1, 2.0f, FColor::Yellow, s); }}
#define P_SCREEN_ERROR(s)			{ if(GEngine) {GEngine->AddOnScreenDebugMessage(-1, 2.0f, FColor::Red, s); }}
#define P_SCREEN_MESSAGE(s, c)		{ if (GEngine) { GEngine->AddOnScreenDebugMessage(-1, 2.0f, c, s); }}

#define P_SCREEN_LOG_T(s, t)			{ if(GEngine) {GEngine->AddOnScreenDebugMessage(-1, t, FColor::Green, s); }}
#define P_SCREEN_WARNING_T(s, t)		{ if(GEngine) {GEngine->AddOnScreenDebugMessage(-1, t, FColor::Yellow, s); }}
#define P_SCREEN_ERROR_T(s, t)			{ if(GEngine) {GEngine->AddOnScreenDebugMessage(-1, t, FColor::Red, s); }}
#define P_SCREEN_MESSAGE_T(s, c, t)		{ if (GEngine) { GEngine->AddOnScreenDebugMessage(-1, t, c, s); }}

class TESTCPP_API LOGME {

public:
	LOGME();
	~LOGME();
};
