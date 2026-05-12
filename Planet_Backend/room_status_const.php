<?php

/**
 * ゲームの状態を定数で管理するクラス
 * @param RoomStatus::WAITING #マッチング待機中
 * @param RoomStatus::PLAYING #ゲーム中
 * @param RoomStatus::FINISHED #終了
 */
class RoomStatus
{
    public const WAITING = 'waiting';   // マッチング待機中
    public const PLAYING = 'playing'; // ゲーム中
    public const FINISHED = 'finished';   // 終了
}
?>