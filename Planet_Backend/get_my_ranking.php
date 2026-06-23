<?php
require_once 'db_connect.php';
require_once 'user.php';

$playerId = $_POST['player_id'];

$stmt = $db->prepare("SELECT COUNT(*) as rank FROM users WHERE best_score > (SELECT best_score FROM users WHERE id = :player_id)");
$stmt->bindValue(':player_id', $playerId, PDO::PARAM_INT);
$stmt->execute();
$row = $stmt->fetch(PDO::FETCH_ASSOC);

if($row !== false){
    $rank = $row['rank'] + 1;
}else{
    $rank = 0;
}

$user = new User($db, $playerId);
$score = $user->get_best_score();
$name = $user->get_user_name();
$icon = $user->get_user_icon();

if($score === 0){
    $rank = 0;
}

echo json_encode([
    'rank' => $rank,
    'score' => $score,
    'name' => $name,
    'icon' => $icon
]);
?>