<?php
require_once 'room.php';
require_once 'user.php';

/**
 * セッション管理クラス
 * 
 * @method __construct(PDO $db, int $roomId) コンストラクタ
 * @method initialize_users() ゲーム開始時の処理。プレイヤーのスコアをリセットする
 */
class SessionManager
{
    private PDO $db;
    private int $roomId;

    public Room $room;   // ルームの情報を格納するRoomクラスのインスタンス

    private const INITIAL_SCORE = 0;   // ゲーム開始時のスコア

    /**
     * コンストラクタ
     * 
     * @param PDO $db データベース接続のPDOインスタンス
     * @param int $roomId ルームID
     */
    public function __construct(PDO $db, int $roomId)
    {
        $this->db = $db;
        $this->roomId = $roomId;
        $this->room = new Room($db, $roomId);
    }

    /**
     * ゲーム開始時の処理
     * プレイヤーのスコアをリセットする
     */
    public function initialize_users()
    {
        foreach ($this->room->users as $user) {
            $user->update_score(self::INITIAL_SCORE);
        }
    }
}