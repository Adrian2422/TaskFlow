import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'withPrefix',
})
/* A helper pipe for constructing complete translation keys.
 * @example <span>{{ 'title' | withPrefix:prefix | translate }}</span>
 */
export class WithPrefixPipe implements PipeTransform {
  /**
   * @param key - detailed key, eg. `'title'`
   * @param prefix - base key, eg. `'user-profile'`
   * @returns combined key eg. `'user-profile.title'`
   */
  public transform(key: string, prefix: string | undefined | null): string {
    if (!prefix) {
      return key;
    }
    return `${prefix}.${key}`;
  }
}
