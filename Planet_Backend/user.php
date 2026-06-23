<?php
/**
 * ユーザークラス
 * 
 * @method get_user_data() ユーザーのデータを取得
 * @method get_user_name() ユーザーの名前を取得
 * @method update_user_name(string $user_name) ユーザーの名前を更新
 * @method update_best_score(int $score) ユーザーのベストスコアを更新
 * @method get_best_score() ユーザーのベストスコアを取得
 */
class User
{
    private PDO $db;
    public int $id;

    public function __construct(PDO $db, int $id)
    {
        $this->db = $db;
        $this->id = $id;
    }

    /**
     * ユーザーのデータを取得
     *
     * @return array ユーザーのデータ
     */
    public function get_user_data()
    {
        $stmt = $this->db->prepare("SELECT * FROM users WHERE id = :id");
        $stmt->bindValue(':id', $this->id, PDO::PARAM_INT);
        $stmt->execute();
        return $stmt->fetch(PDO:: FETCH_ASSOC);
    }

    /**
     * ユーザーの名前を取得
     * 
     * @return string ユーザーの名前
     */
    public function get_user_name()
    {
        $stmt = $this->db->prepare("SELECT user_name FROM users WHERE id = :id");
        $stmt->bindValue(':id', $this->id, PDO::PARAM_INT);
        $stmt->execute();
        return $stmt->fetchColumn();
    }

    /**
     * ユーザーの名前を更新
     *
     * @param string $user_name ユーザーの名前
     */
    public function update_user_name(string $user_name)
    {
        $user_name = empty($user_name) ? '匿名ユーザー' : $user_name;

        $stmt = $this->db->prepare("UPDATE users SET user_name = :user_name WHERE id = :id");
        $stmt->bindValue(':user_name', $user_name, PDO::PARAM_STR);
        $stmt->bindValue(':id', $this->id, PDO::PARAM_INT);
        $stmt->execute();
    }

    /**
     * ユーザーのアイコンを取得
     * 
     * @return string ユーザーのアイコン番号
     */
    public function get_user_icon()
    {
        $stmt = $this->db->prepare("SELECT user_icon FROM users WHERE id = :id");
        $stmt->bindValue(':id', $this->id, PDO::PARAM_INT);
        $stmt->execute();
        return $stmt->fetchColumn();
    }

    /**
     * ユーザーのアイコンを更新
     *
     * @param int $user_icon ユーザーのアイコン番号
     */
    public function update_user_icon(int $user_icon)
    {
        $stmt = $this->db->prepare("UPDATE users SET user_icon = :user_icon WHERE id = :id");
        $stmt->bindValue(':user_icon', $user_icon, PDO::PARAM_INT);
        $stmt->bindValue(':id', $this->id, PDO::PARAM_INT);
        $stmt->execute();
    }

    /**
     * ユーザーのベストスコアを更新
     * 
     * @param int $score ユーザーのベストスコア
     */
    public function update_best_score(int $score)
    {
        // スコアがベストスコア以下の場合は更新しない
        if($score <= $this->get_best_score()){
            return;
        }

        $stmt = $this->db->prepare("UPDATE users SET best_score = :score WHERE id = :id");
        $stmt->bindValue(':score', $score, PDO::PARAM_INT);
        $stmt->bindValue(':id', $this->id, PDO::PARAM_INT);
        $stmt->execute();
    }

    /**
     * ユーザーのベストスコアを取得
     *
     * @return int ユーザーのベストスコア
     */
    public function get_best_score()
    {
        $stmt = $this->db->prepare("SELECT best_score FROM users WHERE id = :id");
        $stmt->bindValue(':id', $this->id, PDO::PARAM_INT);
        $stmt->execute();

        $best_score = $stmt->fetchColumn();
        if($best_score === false){
            // ベストスコアがnullの場合は0を返す
            return 0;
        }

        return $best_score;
    }
}