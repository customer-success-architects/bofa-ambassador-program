// server.rs

mod tcp;

fn main() {
    println!("LOG (MAIN): Starting server");

    let address: &str = "127.0.0.1:8000";

    let tcp_listener: std::net::TcpListener = match std::net::TcpListener::bind(address) {
        Ok(tcp_listener) => tcp_listener,
        Err(e) => panic!("ERROR (MAIN): Unable to bind to {}. Error: {}", address, e),
    };

    match tcp_listener.local_addr() {
        Ok(local_addr) => println!("LOG (MAIN): Server is listening on {}", local_addr),
        Err(e) => println!("WARNING (MAIN): Failed to log the local address: {}", e),
    }

    for tcp_stream in tcp_listener.incoming() {
        match tcp_stream {
            Ok(tcp_stream) => {
                match tcp_stream.local_addr() {
                    Ok(local_addr) => println!("\nLOG (MAIN): New TcpStream Received ({})", local_addr),
                    Err(e) => println!("WARNING (MAIN): Failed to log the local address: {}", e),
                }

                std::thread::spawn(move || {
                    tcp::handle_tcp_stream(tcp_stream);
                });
            }
            Err(e) => {
                println!("ERROR (MAIN): TcpStream Error: {}", e);
            }
        }
    }
}