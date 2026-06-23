<?php
/**
 * ユーザーIDを解決するクラス
 * 
 * @method resolve_id(string $userId) プレイヤーの個別IDを識別してプライマリキーを返す
 * @method create_user_id(int $length = 16) ユーザーIDを生成(デフォルトは16桁)
 * @method save_user_id(string $userId) ユーザーIDをDBに登録し、連番IDを返す
 */
class UserResolver
{
    private PDO $db;

    public function __construct(PDO $db)
    {
        $this->db = $db;
    }

    /**
     * プレイヤーの個別IDを識別してプライマリキーを返す
     *
     * @param string $userId プレイヤーの個別ID
     * @return int プライマリキー
     */
    public function resolve_id(string $userId)
    {
        $stmt = $this->db->prepare("SELECT id FROM users WHERE user_id = :userId");
        $stmt->bindValue(':userId', $userId, PDO::PARAM_STR);
        $stmt->execute();
        $player = $stmt->fetch(PDO::FETCH_ASSOC);

        if(!$player){
            if(empty($userId)){
                // ユーザーIDが空文字だった場合、新しいユーザーIDを生成する
                $userId = $this->create_user_id();
            }
            // ユーザーIDと一致するプレイヤーがいない場合は登録する
            $id = $this->save_user_id($userId);
        }else{
            $id = $player['id'];
        }

        return $id;
    }

    /**
     * ユーザーIDを生成(デフォルトは16桁)
     *
     * @param int $length IDの長さ
     * @return string 生成されたユーザーID
     */
    public function create_user_id(int $length = 16)
    {
        $chars = '23456789abcdefghjkmnpqrstuvwxyz';
        $charsLength = strlen($chars);
        $userId = '';

        for($i = 0; $i < $length; $i++){
            $userId .= $chars[random_int(0, $charsLength - 1)];
        }

        return $userId;
    }

    /**
     * ユーザーIDをDBに登録し、連番IDを返す
     *
     * @param string $userId ユーザーID
     * @return int 連番ID
     */
    private function save_user_id(string $userId)
    {
        try{
            // ユーザーIDを登録
            $stmt = $this->db->prepare("INSERT INTO users(user_id) VALUES(:userId)");
            $stmt->bindValue(':userId', $userId, PDO::PARAM_STR);
            $stmt->execute();
            return $this->db->lastInsertId();
        }catch(PDOException $e){
            // user_idはUNIQUEキーなので、重複していた場合、以下のエラーを吐く
            if($e->getCode() == '23000'){
                // 重複していた場合、新しいIDを生成し、登録する
                return $this->save_user_id($this->create_user_id());
            }else{
                throw $e;
            }
        }
    }
}    
?>