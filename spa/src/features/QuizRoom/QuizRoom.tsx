import { useEffect, useState } from 'react';
import * as signalR from '@microsoft/signalr';

export function QuizRoom() {
  const [connection, setConnection] = useState<signalR.HubConnection | null>(null);
  const [question, setQuestion] = useState('');

  useEffect(() => {
    const newConnection = new signalR.HubConnectionBuilder()
      .withUrl("https://localhost:5000/api") // adjust URL as needed
      .withAutomaticReconnect()
      .build();

    setConnection(newConnection);

    return () => {
      if (newConnection) {
        newConnection.stop();
      }
    };
  }, []);

  useEffect(() => {
    if (connection) {
      connection
        .start()
        .then(() => {
          console.log("SignalR Connected.");

          connection.on("ReceiveQuestion", (receivedQuestion: string) => {
            setQuestion(receivedQuestion);
          });
        })
        .catch((error) => console.error("SignalR Connection Error: ", error));
    }
  }, [connection]);

  const sendAnswer = () => {
    if (connection) {
      connection
        .invoke("SendAnswer", "room123", "MyAnswer")
        .catch((err) => console.error("SendAnswer error: ", err));
    }
  };

  return (
    <div>
      <h2>Question: {question}</h2>
      <button onClick={sendAnswer}>Send Answer</button>
    </div>
  );
}