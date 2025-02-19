# API Application

This API application provides various functionalities including the `GetBestExecutionPlan` function to calculate the optimal execution plan for trading assets across multiple exchanges.

## Table of Contents

- [Installation](#installation)
- [Usage](#usage)
- [Support](#support)
- [Contributing](#contributing)

## Installation

To install and run the API application using Docker there are two options. Run it from docker hub or to build it locally. Note that Api application will be running on port 80 and it must not be used by some other process.

- Run directly from docker hub:
    ```sh
    docker run -p 80:80 mitjafortuna/meta-exchange-api-repo:latest
    ```
- Build and run it locally:
    - Build the Docker image:
        ```sh
        docker build -t meta-exchange-api .
        ```

    - Run the Docker container:
        ```sh
        docker run -d -p 80:80 meta-exchange-api
        ```

## Usage

Once the Docker container is running, you can access the API at `http://localhost/api/metaexchange/execution-plan`. Below are example example of how to use the endpoint.

### Example Usage with Swagger UI

Open Swagger UI on the `http://localhost/swagger/index.html`. Click on `Try it out` button and then click `Execute` button.

### Example Usage wit cURL

1. Make a POST request to the `/api/metaexchange/execution-plan` endpoint with the required parameters:

    ```sh
    curl -X POST http://localhost/api/metaexchange/execution-plan\
    -H "Content-Type: application/json" \
    -d '{
        "orderType": "Buy",
        "orderAmount": 0.5
    }'
    ```

2. The response will contain the optimal execution plan:

    ```json
    [
    {
        "exchangeName": "Dud Bolt exchange 3012",
        "price": 2955.03,
        "amount": 0.5
    }
    ]
    ```

## Support

For support, please open an issue on the [GitHub repository](https://github.com/mitjafortuna/BSDigitalMetaExchangeSolver/issues).

## Contributing

Contributions are welcome! Please fork the repository and submit a pull request. Code can be formatted using the command `dotnet csharpier .` executed in the root of the repository.
