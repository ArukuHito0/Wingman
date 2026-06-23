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

/**
 * ルームクラス
 * 
 * @param array $users ルーム内のプレイヤーのUserクラスのインスタンスを格納する配列
 * 
 * @method get_room_status() ルームの状態を取得する
 * @method update_room_status(string $status) ルームの状態を変更する
 */
class Room
{
    private PDO $db;
    private int $roomId;

    public array $users;   // ルーム内のプレイヤーのUserクラスのインスタンスを格納する配列

    public function __construct(PDO $db, int $roomId)
    {
        $this->db = $db;
        $this->roomId = $roomId;
        
        // ルーム内のプレイヤーのIDを取得
        $stmt = $this->db->prepare("SELECT id FROM users WHERE room_id = :roomId");
        $stmt->bindValue(':roomId', $this->roomId, PDO::PARAM_INT);
        $stmt->execute();
        $playerIds = $stmt->fetchAll(PDO::FETCH_ASSOC);

        $this->users = [];
        foreach ($playerIds as $playerId) {
            $this->users[] = new User($db, $playerId['id']);
        }
    }

    /**
     * ルームの状態を取得する
     */
    public function get_room_status()
    {
        $stmt = $this->db->prepare("SELECT room_status FROM rooms WHERE id = :roomId");
        $stmt->bindValue(':roomId', $this->roomId, PDO::PARAM_INT);
        $stmt->execute();
        return $stmt->fetchColumn();
    }
    
    /**
     * ルームの状態を変更する
     * 
     * @param string $status ルームの状態
     */
    public function update_room_status(string $status)
    {
        $stmt = $this->db->prepare("UPDATE rooms SET room_status = :room_status WHERE id = :roomId");
        $stmt->bindValue(':room_status', $status, PDO::PARAM_STR);
        $stmt->bindValue(':roomId', $this->roomId, PDO::PARAM_INT);
        $stmt->execute();
    }
}
?>