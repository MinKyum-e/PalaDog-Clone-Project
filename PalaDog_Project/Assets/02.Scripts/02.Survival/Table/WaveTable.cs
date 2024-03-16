using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum WaveType                   //웨이브의 종류/일반
{
    Normal ,
    Boss
};
/// <summary>
/// 웨이브 시스템 관리를 위한 웨이브 테이블
/// </summary>
public struct WaveTable
{
    public int Wave_Index;                 //웨이브의 테이블 내 고유 인덱스
    public string Wave_Name;               //웨이브의 이름(스테이지 및 웨이브 순서 명시)
    public string Wave_DevName;            //웨이브의 개발명(스테이지 및 웨이브 순서 명시)
    public int Wave_Group;                 //동일 웨이브 내 몬스터 그룹화를 위한 그룹 인덱스
    public int Wave_StageNum;              //웨이브가 충현하는 스테이지 번호
    public int Wave_WaveNum;               //스테이지 내의 웨이브 순서 번호
    public WaveType Wave_WaveType;
    public float Wave_SpawnTime;           //웨이브의 재출현 주기
    public int Wave_MonsterIndex;          //웨이브에 포함할 몬스터의 고유 인덱스
    public int Wave_MonsterNum;            //웨이브에 포함할 몬스터 종류 별 개체 수
    
}
