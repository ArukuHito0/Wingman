<?php
require_once 'room_status_const.php';

/**
 * ルームマッチングを管理するクラス
 */
class Matching
{
    private PDO $db;

    public function __construct(PDO $db)
    {
        $this->db = $db;
    }

    /**
     * 新規ルーム作成
     * 
     * @param string $password ルームの合言葉
     * @return int 作成されたルームのID
     */
    public function create_room(string $password)
    {
        $stmt = $this->db->prepare("INSERT INTO rooms (password, room_status) VALUES (:password, :room_status)");
        $stmt->bindValue(':password', $password, PDO::PARAM_STR);
        $stmt->bindValue(':room_status', RoomStatus::WAITING, PDO::PARAM_STR);
        $stmt->execute();
        $roomId = (int)$this->db->lastInsertId();

        return $roomId;
    }

    /**
     * ルーム検索(合言葉あり)
     * 
     * @param string $password ルームの合言葉
     * @return int 一致するルームのID。見つからない場合は0を返す
     */
    public function find_password_room(string $password)
    {
        $stmt = $this->db->prepare("SELECT rooms.id, rooms.room_status FROM rooms LEFT JOIN users ON rooms.id = users.room_id
                                    WHERE rooms.room_status = :room_status AND rooms.password = :password
                                    GROUP BY rooms.id HAVING COUNT(users.id) < 2
                                    ORDER BY rooms.id ASC LIMIT 1");
        $stmt->bindValue(':room_status', RoomStatus::WAITING, PDO::PARAM_STR);
        $stmt->bindValue(':password', $password, PDO::PARAM_STR);
        $stmt->execute();
        $room = $stmt->fetch(PDO::FETCH_ASSOC);
        
        return $room ? $room['id'] : 0;
    }

    /**
     * ルーム検索(合言葉なし)
     * 
     * @return int 一致するルームのID。見つからない場合は0を返す
     */
    public function find_free_room()
    {
        $stmt = $this->db->prepare("SELECT rooms.id, rooms.room_status FROM rooms LEFT JOIN users ON rooms.id = users.room_id
                                    WHERE rooms.room_status = :room_status AND rooms.password = ''
                                    GROUP BY rooms.id HAVING COUNT(users.id) < 2
                                    ORDER BY rooms.id ASC LIMIT 1");
        $stmt->bindValue(':room_status', RoomStatus::WAITING, PDO::PARAM_STR);
        $stmt->execute();
        $room = $stmt->fetch(PDO::FETCH_ASSOC);

        return $room ? $room['id'] : 0;
    }

    /**
     * ルーム参加
     * 
     * @param int $roomId ルームのID
     * @param int $playerId プレイヤーの連番ID(プライマリキー)
     */
    public function join_room(int $roomId, int $playerId)
    {
        $stmt = $this->db->prepare("UPDATE users SET room_id = :roomId WHERE id = :playerId");
        $stmt->bindValue(':roomId', $roomId, PDO::PARAM_INT);
        $stmt->bindValue(':playerId', $playerId, PDO::PARAM_INT);
        $stmt->execute();
    }

    /**
     * ルーム退出
     * 
     * @param int $playerId プレイヤーの連番ID(プライマリキー)
     */
    public function leave_room(int $playerId)
    {
        $stmt = $this->db->prepare("UPDATE users SET room_id = 0 WHERE id = :playerId");
        $stmt->bindValue(':playerId', $playerId, PDO::PARAM_INT);
        $stmt->execute();
    }

    /**
     * ルームのプレイヤーが2人揃っていたらtrueを返す
     * 
     * @param int $roomId ルームのID
     * @return bool プレイヤーが2人揃っているか
     */
    public function is_connect_players(int $roomId)
    {
        $stmt = $this->db->prepare("SELECT COUNT(id) AS player_count FROM users WHERE room_id = :roomId");
        $stmt->bindValue(':roomId', $roomId, PDO::PARAM_INT);
        $stmt->execute();
        $result = $stmt->fetch(PDO::FETCH_ASSOC);

        if ($result['player_count'] === 2) {
            return true;
        }else{
            return false;
        }
    }

    /**
     * ルームの状態をゲームプレイ中に変更する
     * 
     * @param int $roomId ルームのID
     */
    public function start_game(int $roomId)
    {
        $stmt = $this->db->prepare("UPDATE rooms SET room_status = :room_status WHERE id = :roomId");
        $stmt->bindValue(':room_status', RoomStatus::PLAYING, PDO::PARAM_STR);
        $stmt->bindValue(':roomId', $roomId, PDO::PARAM_INT);
        $stmt->execute();
    }
}
?>