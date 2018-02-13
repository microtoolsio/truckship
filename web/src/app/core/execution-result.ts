import { ErrorInfo } from "./error-info";

export class ExecutionResult<T> {
  public _success?: boolean;

  get success(): boolean {
    if (this.errors) {
      return this.errors.length === 0;
    }
    return this._success || true;
  }

  set success(value: boolean) {
    this._success = value;
  }

  constructor(public value: T, public errors?: ErrorInfo[]) { }
}
