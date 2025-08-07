# OMS

This project was generated using [Angular CLI](https://github.com/angular/angular-cli) version 7.1.0.

## Getting Started

### 1. Clone the Repository

```bash
git clone https://github.com/Abinaya181102/DotTestKit.git
cd OMSClient
```

### 2. Install Dependencies

```bash
npm install
```

> This will install Angular CLI, Karma, Jasmine, and other dependencies.

If you got an error installing this, try this command.
```bash
npm install --f
```

---

## Running the Application

```bash
ng serve
```

Navigate to: [http://localhost:4200](http://localhost:4200)

> The app will automatically reload if you make any changes to the source files.

If you got an error running this, run this command and then run `ng serve` again.

```bash
$env:NODE_OPTIONS="--openssl-legacy-provider"
```

---

## Running Unit Tests

```bash
ng test
```

- **Test Runner:** [Karma](https://karma-runner.github.io/)
- **Testing Framework:** [Jasmine](https://jasmine.github.io/)

This command will launch a browser and execute all unit tests in watch mode.

If you got an error running this, run this command and then run `ng test` again.

```bash
$env:NODE_OPTIONS="--openssl-legacy-provider"
```

---

---

## Running Unit Tests with Code Coverage

```bash
ng test --code-coverage
```

- **Code Coverage Tool:** [Istanbul](https://istanbul.js.org/) (via `karma-coverage` plugin)
- Coverage report will be generated in the `/coverage` folder.

To view the coverage report:

1. Open the file: `coverage/index.html` in your browser.
2. You'll see color-coded files showing what is and isn’t covered by tests.
