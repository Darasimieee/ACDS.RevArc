const Greeting = (name) => {
    let timeNow = new Date();               // Get the current time
    let greetingMsg = "";                   // Initialize the greeting message
    let hours = timeNow.getHours();         // Get the number of hours

    if (hours >= 0 && hours < 12) {
      greetingMsg = "Good morning, " + name + "!";     // Morning greeting
    } 
    else if (hours >= 12 && hours < 18) {
      greetingMsg = "Good afternoon, " + name + "!";   // Afternoon greeting
    } 
    else {
      greetingMsg = "Good evening, " + name + "!";     // Evening greeting
    }
    return greetingMsg;
}

export default Greeting;