declare module 'amf-client-js' {

  export class ResourceNotFound {
    constructor(error: String)
  }

  export class ResolutionPipeline {
      static DEFAULT_PIPELINE : string
      static EDITING_PIPELINE : string
      static COMPATIBILITY_PIPELINE : string
  }

  export class AMF {
    static init(): Promise<void>

    static raml10Parser(): Raml10Parser

    static ramlParser(): RamlParser

    static raml10Generator(): Raml10Renderer

    static raml08Parser(): Raml08Parser

    static raml08Generator(): Raml08Renderer

    static oas20Parser(): Oas20Parser

    static oas20Generator(): Oas20Renderer

    // static amfGraphParser(): AmfGraphParser

    static amfGraphGenerator(): AmfGraphRenderer

    static validate(model: model.document.BaseUnit, profileName: ProfileName, messageStyle: MessageStyle, env?: client.environment.Environment): Promise<client.validate.ValidationReport>

    static loadValidationProfile(url: string): Promise<string>

    static loadValidationProfile(url: string, env: client.environment.Environment): Promise<string>

    static emitShapesGraph(profileName: ProfileName): string

    static registerNamespace(alias: string, prefix: string): boolean

    // static registerDialect(url: string): Promise<Dialect>

    static resolveRaml10(unit: model.document.BaseUnit): model.document.BaseUnit

    static resolveRaml08(unit: model.document.BaseUnit): model.document.BaseUnit

    static resolveOas20(unit: model.document.BaseUnit): model.document.BaseUnit

    static resolveAmfGraph(unit: model.document.BaseUnit): model.document.BaseUnit

    static jsonPayloadParser(): JsonPayloadParser

    static yamlPayloadParser(): YamlPayloadParser
  }

  export class ProfileNames {
    static AMF: ProfileName
    static OAS: ProfileName
    static OAS3: ProfileName
    static RAML: ProfileName
    static RAML08: ProfileName
  }

  export class ProfileName {
    readonly profile: String
    readonly messageStyle: MessageStyle

    constructor(profile: String)
  }

  export interface MessageStyle {
    profileName: ProfileName
  }

  export class MessageStyles {
    static RAML: MessageStyle
    static OAS: MessageStyle
    static AMF: MessageStyle
  }

  export class AmfGraphParser {
  }

  export class AmfGraphRenderer extends render.Renderer {
    constructor()

    generateToBuilder<T>(unit: model.document.BaseUnit, options: render.RenderOptions, builder: org.yaml.builder.DocBuilder<T>): Promise<void>

    generateToBuilder<T>(unit: model.document.BaseUnit, builder: org.yaml.builder.DocBuilder<T>): Promise<void>
  }

  export class AmfGraphResolver extends resolve.Resolver {
  }

  export class Core {
    static init(): Promise<void>

    static generator(vendor: string, mediaType: string): render.Renderer

    static resolver(vendor: string): resolve.Resolver

    static parser(vendor: string, mediaType: string): parse.Parser

    static validate(model: model.document.BaseUnit, profileName: ProfileName, messageStyle?: MessageStyle): Promise<client.validate.ValidationReport>

    static validate(model: model.document.BaseUnit, profileName: ProfileName, messageStyle: MessageStyle, env: client.environment.Environment): Promise<client.validate.ValidationReport>

    static loadValidationProfile(url: string): Promise<string>

    static loadValidationProfile(url: string, env: client.environment.Environment): Promise<string>

    static emitShapesGraph(profileName: ProfileName): string

    static registerNamespace(alias: string, prefix: string): boolean

    static registerPlugin(plugin: client.plugins.ClientAMFPlugin): void

    static registerPayloadPlugin(plugin: client.plugins.ClientAMFPayloadValidationPlugin): void
  }

  export class JsonPayloadParser extends parse.Parser {
    constructor()
    constructor(env: client.environment.Environment)
  }

  export class JsonldRenderer extends render.Renderer {
    constructor()
  }

  export class Oas20Parser extends parse.Parser {
    constructor()
    constructor(env: client.environment.Environment)
  }

  export class Oas20YamlParser extends parse.Parser {
    constructor()
    constructor(env: client.environment.Environment)
  }

  export class Oas20Renderer extends render.Renderer {
  }

  export class Oas20Resolver extends resolve.Resolver {
  }

  export class Raml08Parser extends parse.Parser {
    constructor()
    constructor(env: client.environment.Environment)
  }

  export class Raml08Renderer extends render.Renderer {
  }

  export class Raml08Resolver extends resolve.Resolver {
  }

  export class Raml10Parser extends parse.Parser {
    constructor()
    constructor(env: client.environment.Environment)
  }

  export class Raml10Renderer extends render.Renderer {
  }

  export class Raml10Resolver extends resolve.Resolver {
  }

  export class RamlParser extends parse.Parser {
    constructor()
    constructor(env: client.environment.Environment)
  }

  export class VocabulariesParser extends parse.Parser {
    constructor()
    constructor(env: client.environment.Environment)
  }

  export class YamlPayloadParser extends parse.Parser {
    constructor()
    constructor(env: client.environment.Environment)
  }

  export class ValidationMode {
    static StrictValidationMode: ValidationMode
    static ScalarRelaxedValidationMode: ValidationMode
  }

  /* Not exported */
  export abstract class BaseHttpResourceLoader implements resource.ResourceLoader {
    accepts(resource: string): boolean

    abstract fetch(resource: string): Promise<client.remote.Content>
  }

  export abstract class BaseFileResourceLoader implements resource.ResourceLoader {
      accepts(resource: string): boolean
      abstract fetch(resource: string): Promise<client.remote.Content>
  }

  export class JsBrowserHttpResourceLoader extends BaseHttpResourceLoader {
    fetch(resource: string): Promise<client.remote.Content>
  }

  export class JsServerFileResourceLoader extends BaseFileResourceLoader {
    fetch(resource: string): Promise<client.remote.Content>
  }

  namespace org {

    namespace mulesoft {

      namespace common {

        namespace io {

          export class LimitedStringBuffer {

            constructor(limit: number)

            length: number

            toString(): string
          }

          class LimitReachedException {
          }
        }
      }
    }

    namespace yaml {
      namespace builder {

        /* Not exported */
        abstract class DocBuilder<T> {
          result: T

          isDefined: boolean

          list(f: (p: DocBuilder.Part) => void): T

          obj(f: (e: DocBuilder.Entry) => void): T

          doc(f: (p: DocBuilder.Part) => void): T
        }

        namespace DocBuilder {
          /* Not exported */
          abstract class Part {

          }

          /* Not exported */
          abstract class Entry {

          }
        }

        class JsOutputBuilder extends DocBuilder<any> {

        }
      }
    }
  }

  namespace client {

    export class DefaultEnvironment {
      static apply(): environment.Environment
    }

    namespace environment {
      export class Environment {

        constructor()

        loaders: resource.ResourceLoader[]

        addClientLoader(loader: resource.ResourceLoader): Environment

        withLoaders(loaders: resource.ResourceLoader[]): Environment

        static empty(): Environment

        static apply(loader: resource.ResourceLoader): Environment
      }
    }

    namespace remote {
      export class Content {
        stream: string
        url: string

        constructor(stream: string, url: string)

        constructor(stream: string, url: string, mime: string)
      }
    }

    namespace plugins {
      interface ClientAMFPlugin {
        ID: string

        dependencies(): ClientAMFPlugin[]

        init(): Promise<ClientAMFPlugin>
      }

      interface ClientAMFPayloadValidationPlugin extends ClientAMFPlugin {
        payloadMediaType: String[]

        canValidate(shape: model.domain.Shape, env: client.environment.Environment): boolean

        validator(shape: model.domain.Shape, env: client.environment.Environment, validationMode: ValidationMode): ClientPayloadValidator
      }

      interface ClientPayloadValidator {
        shape: model.domain.Shape
        defaultSeverity: string
        validationMode: ValidationMode
        env: client.environment.Environment

        validate(payload: string, mediaType: string): Promise<client.validate.ValidationReport>

        validate(payloadFragment: model.domain.PayloadFragment): Promise<client.validate.ValidationReport>

        isValid(payload: string, mediaType: string): Promise<Boolean>
      }

      export class ValidationShapeSet {
        constructor(candidates: ValidationCandidate[], defaultSeverity: string)

        candidates: ValidationCandidate[]
        defaultServerity: string
      }

      export class ValidationCandidate {
        constructor(shape: model.domain.Shape, payload: model.domain.PayloadFragment)

        shape: model.domain.Shape
        payload: model.domain.PayloadFragment
      }
    }

    namespace validate {

      export class PayloadParsingResult {
        constructor(fragment: model.domain.PayloadFragment, results: ValidationResult[])

        fragment: model.domain.PayloadFragment
        results: ValidationResult[]
      }

      export class ValidationReport {
        constructor(conforms: boolean, model: string, profile: ProfileName, results: ValidationResult[])

        conforms: boolean
        model: string
        profile: ProfileName
        results: ValidationResult[]
      }

      export class ValidationResult {
        constructor(message: string, level: string, targetNode: string, targetProperty: string, validationId: string, position: core.parser.Range, location: string)

        message: string
        level: string
        targetNode: string
        targetProperty: string
        validationId: string
        position: core.parser.Range
        source: any
        location: string
      }

    }
  }

  namespace model {
    /* Not exported */
    interface Annotable {
      annotations(): Annotations
    }

    /* Not exported */
    interface BaseField extends Annotable {
    }

    /* Not exported */
    class Annotations {
      lexical(): core.parser.Range

      custom(): model.domain.DomainExtension[]
    }

    /* Not exported */
    abstract class ValueField<T> {
      abstract option: T | undefined

      abstract value(): T

      is(other: T): boolean

      is(accepts: (t: T) => boolean): boolean

      isNull: boolean

      nonNull: boolean

      toString: string

      abstract remove(): void
    }

    /* Not exported */
    class StrField extends ValueField<string> implements BaseField {
      option: string | undefined

      annotations(): Annotations

      value(): string

      isNullOrEmpty: boolean

      nonEmpty: boolean

      remove(): void
    }

    /* Not exported */
    class BoolField extends ValueField<boolean> implements BaseField {
      option: boolean | undefined

      annotations(): Annotations

      value(): boolean

      remove(): void
    }

    /* Not exported */
    class IntField extends ValueField<number> implements BaseField {
      option: number | undefined

      annotations(): Annotations

      value(): number

      remove(): void
    }

    /* Not exported */
    class DoubleField extends ValueField<number> implements BaseField {
      option: number | undefined

      annotations(): Annotations

      value(): number

      remove(): void
    }

    namespace document {

      /* Not exported */
      abstract class EncodesModel {
        encodes: domain.DomainElement

        withEncodes(enocdes: domain.DomainElement): this
      }

      /* Not exported */
      abstract class DeclaresModel {
        declares: domain.DomainElement[]

        withDeclaredElement(declared: domain.DomainElement): this

        withDeclares(declares: domain.DomainElement[]): this
      }

      /* Not exported */
      abstract class BaseUnit {
        id: string

        references(): BaseUnit[]

        raw: string | undefined

        location: string

        usage: StrField

        withReferences(references: BaseUnit[]): this

        withRaw(raw: string): this

        withLocation(location: string): this

        withUsage(usage: string): this

        findById(id: string): domain.DomainElement | undefined

        findByType(typeId: string): domain.DomainElement[]
      }

      /* Not exported */
      abstract class BaseUnitWithDeclaresModel extends BaseUnit implements DeclaresModel {
        /* DeclaresModel methods */
        declares: domain.DomainElement[]

        withDeclaredElement(declared: domain.DomainElement): this

        withDeclares(declares: domain.DomainElement[]): this
      }

      /* Not exported */
      abstract class BaseUnitWithEncodesModel extends BaseUnit implements EncodesModel {
        /* EncodesModel methods */
        encodes: domain.DomainElement

        withEncodes(enocdes: domain.DomainElement): this
      }

      /* Not exported */
      abstract class BaseUnitWithDeclaresModelAndEncodesModel extends BaseUnit implements DeclaresModel, EncodesModel {
        /* DeclaresModel methods */
        declares: domain.DomainElement[]

        withDeclaredElement(declared: domain.DomainElement): this

        withDeclares(declares: domain.DomainElement[]): this

        /* EncodesModel methods */
        encodes: domain.DomainElement

        withEncodes(enocdes: domain.DomainElement): this
      }

      export class Document extends BaseUnitWithDeclaresModelAndEncodesModel {
        declares: domain.DomainElement[]
        encodes: domain.DomainElement

        withEncodes(enocdes: domain.DomainElement): this;
      }

      /* Not exported */
      class Fragment extends BaseUnitWithEncodesModel {
      }

      export class ExternalFragment extends Fragment {
      }

      export class Module extends BaseUnitWithDeclaresModel {
      }

      export class Extension extends Fragment {
      }

      export class Overlay extends Fragment {
      }

      export class DocumentationItem extends Fragment {
      }

      export class DataType extends Fragment {
      }

      export class NamedExample extends Fragment {
      }

      export class ResourceTypeFragment extends Fragment {
      }

      export class TraitFragment extends Fragment {
      }

      export class AnnotationTypeDeclaration extends Fragment {
      }

      export class SecuritySchemeFragment extends Fragment {
      }

      export class Vocabulary extends BaseUnit {
        name: StrField
        description: StrField

        base: StrField
        imports: domain.VocabularyReference[]
        externals: domain.External[]

        withName(name: string): Vocabulary

        withBase(base: string): Vocabulary

        withExternals(externals: domain.External[]): Vocabulary

        withImports(vocabularies: domain.VocabularyReference[]): Vocabulary

        objectPropertyTerms(): domain.ObjectPropertyTerm[]

        datatypePropertyTerms(): domain.DatatypePropertyTerm[]

        classTerms(): domain.ClassTerm[]
      }
    }

    namespace domain {

      /* Not exported */
      abstract class DomainElement implements Annotable {
        customDomainProperties: DomainExtension[]
        extendsNode: DomainElement[]
        id: string
        position: core.parser.Range

        annotations(): any

        withCustomDomainProperties(extensions: DomainExtension[]): this

        withExtendsNode(extension: ParametrizedDeclaration[]): this

        withId(id: string): this

        graph(): Graph
      }

      export class Graph {
        types(): string[]

        properties(): string[]

        scalarByProperty(id: string): any[]

        getObjectByPropertyId(id: string): DomainElement[]

        remove(uri: string): this
      }

      export class ClassTerm extends DomainElement {
        name: StrField

        displayName: StrField

        description: StrField

        properties: StrField[]

        subClassOf: StrField[]

        withName(name: string): this

        withDisplayName(displayName: string): this

        withDescription(description: string): this

        withProperties(properties: string[]): this

        withSubClassOf(superClasses: string[]): this

      }

      export class VocabularyReference extends DomainElement {
        alias: StrField
        reference: StrField

        withAlias(alias: string): VocabularyReference

        withReference(reference: string): VocabularyReference
      }

      export class External extends DomainElement {
        alias: StrField
        base: StrField

        withAlias(alias: string): External

        withBase(base: string): External
      }

      export class PropertyTerm extends DomainElement {
        name: StrField
        displayName: StrField
        description: StrField
        range: StrField
        subPropertyOf: StrField[]

        withName(name: string): this

        withDisplayName(displayName: string): this

        withDescription(description: string): this

        withRange(range: string): this
      }

      export class ObjectPropertyTerm extends PropertyTerm {
      }

      export class DatatypePropertyTerm extends PropertyTerm {
      }

      export class CustomDomainProperty extends DomainElement implements Linkable {
        name: StrField

        displayName: StrField

        description: StrField

        domain: StrField[]

        schema: Shape

        withName(name: string): this

        withDisplayName(displayName: string): this

        withDescription(description: string): this

        withDomain(domain: string[]): this

        withSchema(schema: Shape): this

        /* Linkable methods */
        linkTarget: DomainElement | undefined

        isLink: boolean

        linkLabel: StrField

        linkCopy(): Linkable

        withLinkTarget(target: Linkable): this

        withLinkLabel(label: string): this

        link(): CustomDomainProperty
      }

      export class ExternalDomainElement extends DomainElement {
        raw: StrField
        mediaType: StrField

        withRaw(raw: String): this

        withMediaType(mediaType: String): this
      }

      export abstract class Linkable {
        linkTarget: DomainElement | undefined

        isLink: boolean

        linkLabel: StrField

        linkCopy(): Linkable

        withLinkTarget(target: Linkable): this

        withLinkLabel(label: string): this
      }

      export class PayloadFragment extends document.Fragment {
        dataNode: DataNode
        mediaType: string

        constructor(scalar: model.domain.ScalarNode, mediaType: string)
        constructor(object: model.domain.ObjectNode, mediaType: string)
        constructor(array: model.domain.ArrayNode, mediaType: string)
      }

      export class Shape extends DomainElement implements Linkable {
        name: StrField
        displayName: StrField
        description: StrField
        defaultValue: DataNode
        defaultValueStr: StrField

        values: StrField[]
        location: string
        inherits: Shape[]
        or: Shape[]
        and: Shape[]
        xone: Shape[]
        not: Shape

        withName(name: string): this

        withDisplayName(name: string): this

        withDescription(description: string): this

        withDefaultValue(default_: DataNode): this

        withValues(values: string[]): this

        withInherits(inherits: Shape[]): this

        withOr(subShapes: Shape[]): this

        withAnd(subShapes: Shape[]): this

        withXone(subShapes: Shape[]): this

        withNode(shape: Shape): this

        withDefaultStr(value: string): this

        /* Linkable methods */
        linkTarget: DomainElement | undefined

        isLink: boolean

        linkLabel: StrField

        linkCopy(): Linkable

        withLinkTarget(target: Linkable): this

        withLinkLabel(label: string): this

        link(): Shape
      }

      export class PropertyShape extends Shape {
        path: StrField
        range: Shape
        minCount: IntField
        maxCount: IntField
        readOnly: BoolField
        writeOnly: BoolField
        deprecated: BoolField
        patternName: StrField

        withPath(path: string): this

        withRange(range: Shape): this

        withMinCount(min: number): this

        withMaxCount(max: number): this

        withReadOnly(readOnly: boolean): this

        withWriteOnly(writeOnly: boolean): this

        withDeprecated(deprecated: boolean): this

        withPatternName(pattern: string): this
      }

      export class AnyShape extends Shape {
        documentation: CreativeWork
        xmlSerialization: XMLSerializer
        examples: Example[]

        withDocumentation(documentation: CreativeWork): this

        withXMLSerialization(xmlSerialization: XMLSerializer): this

        withExamples(examples: Example[]): this

        withExample(mediaType: string): Example

        linkCopy(): AnyShape

        toJsonSchema: string

        /** RAML Data Type representation. */
        toRamlDatatype: string

        validate(payload: string): Promise<client.validate.ValidationReport>

        validate(fragment: model.domain.PayloadFragment): Promise<client.validate.ValidationReport>

        isDefaultEmpty: string
      }

      export class XMLSerializer extends DomainElement {
        attribute: BoolField
        wrapped: BoolField
        name: StrField
        namespace: StrField
        prefix: StrField

        withAttribute(attribute: boolean): this;

        withWrapped(wrapped: boolean): this;

        withName(name: string): this;

        withNamespace(namespace: string): this;

        withPrefix(prefix: string): this;
      }

      export class ArrayShape extends DataArrangeShape {
        items: Shape

        withItems(items: Shape): this
      }

      export class MatrixShape extends ArrayShape {
        withItems(items: Shape): this
      }

      export class DataArrangeShape extends AnyShape {
        minItems: IntField
        maxItems: IntField
        uniqueItems: BoolField

        withMinItems(minItems: number): this

        withMaxItems(maxItems: number): this

        withUniqueItems(uniqueItems: boolean): this
      }

      export class CreativeWork extends DomainElement {
        url: StrField
        description: StrField
        title: StrField

        withUrl(url: string): this

        withTitle(title: string): this

        withDescription(description: string): this
      }

      export class UnionShape extends AnyShape {
        anyOf: Shape[]

        withAnyOf(anyOf: Shape[]): UnionShape
      }

      export class RecursiveShape extends Shape {
        fixPoint: StrField

        withFixPoint(shapeId: string): this
      }

      export class DataNode extends DomainElement {
        name: StrField

        withName(name: string): this
      }

      export class DomainExtension extends DomainElement {
        name: StrField
        definedBy: CustomDomainProperty
        extension: DataNode

        withName(name: string): this

        withDefinedBy(property: CustomDomainProperty): this

        withExtension(node: DataNode): this
      }

      export class AbstractDeclaration extends DomainElement implements Linkable {
        name: StrField
        description: StrField
        dataNode: DataNode
        variables: StrField[]

        withName(name: string): this

        withDescription(description: string): this

        withDataNode(dataNode: DataNode): this

        withVariables(variables: string[]): this

        /* Linkable methods */
        linkTarget: DomainElement | undefined

        isLink: boolean

        linkLabel: StrField

        linkCopy(): Linkable

        withLinkTarget(target: Linkable): this

        withLinkLabel(label: string): this

        link(): AbstractDeclaration
      }

      export class Trait extends AbstractDeclaration {
      }

      export class ResourceType extends AbstractDeclaration {
      }

      export class ParametrizedDeclaration extends DomainElement {
        name: StrField
        target: AbstractDeclaration
        variables: VariableValue[]

        withName(name: string): this

        withTarget(target: AbstractDeclaration): this

        withVariables(variables: VariableValue[]): this
      }

      export class Variable {
        name: string

        value: DataNode
      }

      export class VariableValue extends DomainElement {
        name: StrField
        value: DataNode

        withName(name: string): this

        withValue(value: DataNode): this
      }

      export class Server extends DomainElement {
        url: StrField
        description: StrField
        variables: Parameter[]

        withUrl(url: string): this

        withDescription(description: string): this

        withVariables(variables: Parameter[]): this

        withVariable(name: string): Parameter
      }

      export class EndPoint extends DomainElement {
        name: StrField
        description: StrField
        summary: StrField
        path: StrField
        operations: Operation[]
        parameters: Parameter[]
        payloads: Payload[]
        servers: Server[]
        security: ParametrizedSecurityScheme[]

        relativePath: string

        withName(name: string): this

        withDescription(description: string): this

        withSummary(summary: string): this

        withPath(path: string): this

        withOperations(operations: Operation[]): this

        withParameters(parameters: Parameter[]): this

        withPayloads(payloads: Payload[]): this

        withServers(servers: Server[]): this

        withSecurity(security: ParametrizedSecurityScheme[]): this

        withOperation(method: string): Operation

        withParameter(name: string): Parameter

        withPayload(name: string): Payload

        withServer(url: string): Server
      }

      export class ParametrizedSecurityScheme extends DomainElement {
        name: StrField
        scheme: SecurityScheme
        settings: Settings

        withName(name: string): this

        withScheme(scheme: SecurityScheme): this

        withSettings(settings: Settings): this

        withDefaultSettings(): Settings

        withOAuth1Settings(): OAuth1Settings

        withOAuth2Settings(): OAuth2Settings

        withApiKeySettings(): ApiKeySettings
      }

      export class SecurityScheme extends DomainElement implements Linkable {
        name: StrField
        type: StrField
        displayName: StrField
        description: StrField
        headers: Parameter[]
        queryParameters: Parameter[]
        responses: Response[]
        settings: Settings
        queryString: Shape

        withName(name: string): this

        withType(type: string): this

        withDisplayName(displayName: string): this

        withDescription(description: string): this

        withHeaders(headers: Parameter[]): this

        withQueryParameters(queryParameters: Parameter[]): this

        withResponses(responses: Response[]): this

        withSettings(settings: Settings): this

        withQueryString(queryString: Shape): this

        withHeader(name: string): Parameter

        withQueryParameter(name: string): Parameter

        withResponse(name: string): Response

        withDefaultSettings(): Settings

        withOAuth1Settings(): OAuth1Settings

        withOAuth2Settings(): OAuth2Settings

        withApiKeySettings(): ApiKeySettings

        withHttpSettings(): HttpSettings

        withOpenIdConnectSettings(): OpenIdConnectSettings

        /* Linkable methods */
        linkTarget: DomainElement | undefined

        isLink: boolean

        linkLabel: StrField

        linkCopy(): Linkable

        withLinkTarget(target: Linkable): this

        withLinkLabel(label: string): this

        link(): SecurityScheme
      }

      export class ParametrizedResourceType extends ParametrizedDeclaration {
      }

      export class ParametrizedTrait extends ParametrizedDeclaration {
      }

      export class Parameter extends DomainElement {
        name: StrField
        description: StrField
        required: BoolField
        deprecated: BoolField
        allowEmptyValue: BoolField
        style: StrField
        explode: BoolField
        allowReserved: BoolField
        binding: StrField
        schema: Shape
        payloads: Payload[]
        examples: Example[]

        withName(name: string): this

        withDescription(description: string): this

        withRequired(required: boolean): this

        withDeprecated(deprecated: boolean): this

        withAllowEmptyValue(allowEmptyValue: boolean): this

        withStyle(style: string): this

        withExplode(explode: boolean): this

        withAllowReserved(allowReserved: boolean): this

        withBinding(binding: string): this

        withPayloads(payloads: Payload[]): this

        withExamples(examples: Example[]): this

        withSchema(schema: Shape): this

        withObjectSchema(name: string): NodeShape

        withScalarSchema(name: string): ScalarShape

        withPayload(mediaType: string): Payload

        withExample(name: string): Example
      }

      export class Example extends DomainElement implements Linkable {
        name: StrField
        displayName: StrField
        description: StrField
        value: StrField
        structuredValue: DataNode
        strict: BoolField
        mediaType: StrField

        withName(name: string): this

        withDisplayName(displayName: string): this

        withDescription(description: string): this

        withValue(value: string): this

        withStructuredValue(value: DataNode): this

        withStrict(strict: boolean): this

        withMediaType(mediaType: string): this

        linkCopy(): Example

        /* Linkable methods */
        linkTarget: DomainElement | undefined

        isLink: boolean

        linkLabel: StrField

        linkCopy(): Linkable

        withLinkTarget(target: Linkable): this

        withLinkLabel(label: string): this

        link(): Example
      }

      export class FileShape extends AnyShape {
        fileTypes: StrField[]
        pattern: StrField
        minLength: IntField
        maxLength: IntField
        minimum: DoubleField
        maximum: DoubleField
        exclusiveMinimum: BoolField
        exclusiveMaximum: BoolField
        format: StrField
        multipleOf: DoubleField

        withFileTypes(fileTypes: string[]): this

        withPattern(pattern: string): this

        withMinLength(min: number): this

        withMaxLength(max: number): this

        withMinimum(min: number): this

        withMaximum(max: number): this

        withExclusiveMinimum(min: boolean): this

        withExclusiveMaximum(max: boolean): this

        withFormat(format: string): this

        withMultipleOf(multiple: number): this
      }

      export class License extends DomainElement {
        url: StrField
        name: StrField

        withUrl(url: string): this

        withName(name: string): this
      }

      export class NilShape extends AnyShape {
      }

      export class NodeShape extends AnyShape {
        minProperties: IntField
        maxProperties: IntField
        closed: BoolField
        discriminator: StrField
        discriminatorValue: StrField
        properties: PropertyShape[]
        dependencies: PropertyDependencies[]

        withMinProperties(min: number): this

        withMaxProperties(max: number): this

        withClosed(closed: boolean): this

        withDiscriminator(discriminator: string): this

        withDiscriminatorValue(value: string): this

        withProperties(properties: PropertyShape[]): this

        withProperty(name: string): PropertyShape

        withDependencies(dependencies: PropertyDependencies[]): this

        withDependency(): PropertyDependencies

        withInheritsObject(name: string): NodeShape

        withInheritsScalar(name: string): ScalarShape

      }

      export class PropertyDependencies extends DomainElement {
        source: StrField
        target: StrField[]

        withPropertySource(propertySource: string): this

        withPropertyTarget(propertyTarget: string[]): this
      }

      export class Request extends DomainElement {
        description: StrField
        required: BoolField
        queryParameters: Parameter[]
        headers: Parameter[]
        payloads: Payload[]
        queryString: Shape
        uriParameters: Parameter[]
        cookieParameters: Parameter[]

        withDescription(description: string): this

        withRequired(required: boolean): this

        withQueryParameters(parameters: Parameter[]): this

        withHeaders(headers: Parameter[]): this

        withPayloads(payloads: Payload[]): this

        withQueryString(queryString: Shape): this

        withUriParameters(uriParameters: Parameter[]): this

        withCookieParameters(cookieParameters: Parameter[]): this

        withQueryParameter(name: string): Parameter

        withHeader(name: string): Parameter

        withPayload(): Payload

        withPayload(mediaType: string): Payload

        withUriParameter(name: string): Parameter

        withCookieParameter(name: string): Parameter
      }

      export class Settings extends DomainElement {
        additionalProperties: DataNode

        withAdditionalProperties(properties: DataNode): this
      }

      export class OAuth1Settings extends Settings {
        requestTokenUri: StrField
        authorizationUri: StrField
        tokenCredentialsUri: StrField
        signatures: StrField[]

        withRequestTokenUri(requestTokenUri: string): this

        withAuthorizationUri(authorizationUri: string): this

        withTokenCredentialsUri(tokenCredentialsUri: string): this

        withSignatures(signatures: string[]): this
      }

      export class OAuth2Settings extends Settings {
        authorizationUri: StrField
        accessTokenUri: StrField
        authorizationGrants: StrField[]
        flow: StrField
        refreshUri: StrField
        scopes: Scope[]

        withAuthorizationUri(uri: string): this

        withAccessTokenUri(token: string): this

        withAuthorizationGrants(grants: string[]): this

        withFlow(flow: string): this

        withRefreshUri(refreshUri: string): this

        withScopes(scopes: Scope[]): this
      }

      export class ApiKeySettings extends Settings {
        name: StrField
        in: StrField

        withName(name: string): this

        withIn(in_: string): this
      }

      export class HttpSettings extends Settings {
        scheme: StrField
        bearerFormat: StrField

        withScheme(scheme: string): this

        withBearerFormat(bearerFormat: string): this
      }

      export class OpenIdConnectSettings extends Settings {
        url: StrField

        withUrl(url: string): this
      }

      export class Scope extends DomainElement {
        name: StrField
        description: StrField

        withName(name: string): this

        withDescription(description: string): this
      }

      export class Callback extends DomainElement {
        name: StrField
        expression: StrField
        endpoint: EndPoint

        withName(name: string): this

        withExpression(expression: string): this

        withEndpoint(endpoint: EndPoint): this

        withEndpoint(): EndPoint
      }

      export class Operation extends DomainElement {
        method: StrField
        name: StrField
        description: StrField
        deprecated: BoolField
        summary: StrField
        documentation: CreativeWork
        schemes: StrField[]
        accepts: StrField[]
        contentType: StrField[]
        request: Request
        responses: Response[]
        security: DomainElement[]
        callbacks: Callback[]
        servers: Server[]

        withMethod(method: string): this

        withName(name: string): this

        withDescription(description: string): this

        withDeprecated(deprecated: boolean): this

        withSummary(summary: string): this

        withDocumentation(documentation: CreativeWork): this

        withSchemes(schemes: string[]): this

        withAccepts(accepts: string[]): this

        withContentType(contentType: string[]): this

        withRequest(request: Request): this

        withResponses(responses: Response[]): this

        withSecurity(security: DomainElement[]): this

        withCallbacks(callbacks: Callback[]): this

        withServers(servers: Server[]): this

        withResponse(name: string): Response

        withRequest(): Request

        withCallback(name: string): Callback

        withServer(name: string): Server
      }

      export class IriTemplateMapping extends DomainElement {
        templateVariable: StrField
        linkExpression: StrField

        withTemplateVariable(variable: string): this

        withLinkExpression(expression: string): this
      }

      export class TemplatedLink extends DomainElement {
        name: StrField
        description: StrField
        template: StrField
        operationId: StrField
        mapping: IriTemplateMapping
        requestBody: StrField
        server: Server

        withName(name: string): this

        withDescription(description: string): this

        withTemplate(template: string): this

        withOperationId(operationId: string): this

        withMapping(mapping: IriTemplateMapping): this

        withRequestBody(requestBody: string): this

        withServer(server: Server): this
      }

      export class Response extends DomainElement {
        name: StrField
        description: StrField
        statusCode: StrField
        headers: Parameter[]
        payloads: Payload[]
        examples: Example[]
        links: TemplatedLink[]

        withName(name: string): this

        withDescription(description: string): this

        withStatusCode(statusCode: string): this

        withHeaders(headers: Parameter[]): this

        withPayloads(payloads: Payload[]): this

        withExamples(examples: Example[]): this

        withLinks(links: TemplatedLink[]): this

        withHeader(name: string): Parameter

        withPayload(): Payload

        withPayload(mediaType: string): Payload
      }

      export class Organization extends DomainElement {
        url: StrField
        name: StrField
        email: StrField

        withUrl(url: string): this

        withName(name: string): this

        withEmail(email: string): this
      }

      export class Encoding extends DomainElement {
        propertyName: StrField
        contentType: StrField
        headers: Parameter[]
        style: StrField
        explode: BoolField
        allowReserved: BoolField

        withPropertyName(propertyName: string): this

        withContentType(contentType: string): this

        withHeaders(headers: Parameter[]): this

        withStyle(style: string): this

        withExplode(explode: boolean): this

        withAllowReserved(allowReserved: boolean): this

        withHeader(name: string): Parameter
      }

      export class Payload extends DomainElement {
        name: StrField
        mediaType: StrField
        schema: Shape
        examples: Example[]
        encoding: Encoding[]

        withName(name: string): this

        withMediaType(mediaType: string): this

        withSchema(schema: Shape): this

        withExamples(examples: Example[]): this

        withEncoding(encoding: Encoding[]): this

        withObjectSchema(name: string): NodeShape

        withScalarSchema(name: string): ScalarShape

        withExample(name: string): Example

        withEncoding(name: string): Encoding

      }

      export class ScalarShape extends AnyShape {
        dataType: StrField
        pattern: StrField
        minLength: IntField
        maxLength: IntField
        minimum: DoubleField
        maximum: DoubleField
        exclusiveMinimum: BoolField
        exclusiveMaximum: BoolField
        format: StrField
        multipleOf: DoubleField

        withDataType(dataType: string): this

        withPattern(pattern: string): this

        withMinLength(min: number): this

        withMaxLength(max: number): this

        withMinimum(min: number): this

        withMaximum(max: number): this

        withExclusiveMinimum(min: boolean): this

        withExclusiveMaximum(max: boolean): this

        withFormat(format: string): this

        withMultipleOf(multiple: number): this

        linkCopy(): ScalarShape
      }

      export class SchemaShape extends AnyShape {
        mediaType: StrField
        raw: StrField

        withMediatype(mediaType: string): this

        withRaw(text: string): this
      }

      export class TupleShape extends DataArrangeShape {
        items: Shape[]

        withItems(items: Shape[]): this

        additionalItems: BoolField

        withAdditionalItems(additionalItems: boolean): this
      }

      export class ObjectNode extends DataNode {
        properties: { [key: string]: DataNode }

        addProperty(property: string, node: DataNode): this
      }

      export class ScalarNode extends DataNode {
        constructor()

        constructor(value: string, dataType: string)

        value: string
        dataType: string

        toString(): string

        static build(value: string, dataType: string): ScalarNode
      }

      export class ArrayNode extends DataNode {
        members: DataNode[]

        addMember(member: DataNode): this

      }

      export class WebApi extends DomainElement {
        name: StrField
        description: StrField
        schemes: StrField[]
        endPoints: EndPoint[]
        accepts: StrField[]
        contentType: StrField[]
        version: StrField
        termsOfService: StrField
        provider: Organization
        license: License
        documentations: CreativeWork[]
        servers: Server[]
        security: ParametrizedSecurityScheme[]

        withName(name: string): this

        withDescription(description: string): this

        withSchemes(schemes: string[]): this

        withEndPoints(endPoints: EndPoint[]): this

        withAccepts(accepts: string[]): this

        withContentType(contentType: string[]): this

        withVersion(version: string): this

        withTermsOfService(terms: string): this

        withProvider(provider: Organization): this

        withLicense(license: License): this

        withDocumentation(documentations: CreativeWork[]): this

        withServers(servers: Server[]): this

        withSecurity(security: ParametrizedSecurityScheme[]): this

        withDocumentationTitle(title: string): CreativeWork

        withDocumentationUrl(url: string): CreativeWork

        withEndPoint(path: string): EndPoint

        withServer(url: string): Server

        withDefaultServer(url: string): Server
      }

    }
  }

  namespace plugins {

    namespace document {

      export class WebApi {
        static register(): any

        static validatePayload(shape: model.domain.Shape, payload: model.domain.DataNode): Promise<client.validate.ValidationReport>
      }

      export class Vocabularies {
        static register(): any
      }
    }

    namespace features {
      export class AMFValidation {
        static register(): any
      }
    }
  }

  namespace render {
    export class RenderOptions {

      withSourceMaps: RenderOptions

      withoutSourceMaps: RenderOptions

      isWithSourceMaps: boolean

      withCompactUris: RenderOptions

      withoutCompactUris: RenderOptions

      isWithCompactUris: boolean

      static apply(): RenderOptions
    }

    /* Not exported */
    class Renderer {

      constructor(vendor: string, mediaType: string)

      generateFile(unit: model.document.BaseUnit, url: string, handler: handler.FileHandler): void

      generateFile(unit: model.document.BaseUnit, url: string, options: RenderOptions, handler: handler.FileHandler): void

      generateString(unit: model.document.BaseUnit, handler: handler.JsHandler<string>): void

      generateString(unit: model.document.BaseUnit, options: RenderOptions, handler: handler.JsHandler<String>): void

      generateFile(unit: model.document.BaseUnit, url: string): Promise<void>

      generateFile(unit: model.document.BaseUnit, url: string, options: RenderOptions): Promise<void>

      generateString(unit: model.document.BaseUnit): Promise<string>

      generateString(unit: model.document.BaseUnit, options: RenderOptions): Promise<string>

      generateToWriter(unit: model.document.BaseUnit, buffer: org.mulesoft.common.io.LimitedStringBuffer): Promise<void>

      generateToWriter(unit: model.document.BaseUnit, options: RenderOptions, buffer: org.mulesoft.common.io.LimitedStringBuffer): Promise<void>
    }
  }

  /* Not exported */
  namespace handler {
    /* Not exported */
    interface JsHandler<T> {
      success(t: T): void;

      error(e: any): void;
    }

    /* Not exported */
    interface FileHandler {
      success(): void;

      error(e: any): void;
    }
  }

  /* Not exported */
  namespace parse {

    /* Not exported */
    class Parser {
      constructor(vendor: string, mediaType: string)

      constructor(vendor: string, mediaType: string, env: any)

      parseFile(url: string, handler: handler.JsHandler<model.document.BaseUnit>): void

      parseString(stream: string, handler: handler.JsHandler<model.document.BaseUnit>): void

      parseFileAsync(url: string): Promise<model.document.BaseUnit>

      parseStringAsync(url: string, stream: string): Promise<model.document.BaseUnit>

      parseStringAsync(stream: string): Promise<model.document.BaseUnit>

      reportValidation(profileName: string, messageStyle: string): Promise<client.validate.ValidationReport>

      reportCustomValidation(profileName: string, customProfilePath: string): Promise<client.validate.ValidationReport>
    }
  }

  /* Not exported */
  namespace resolve {

    /* Not exported */
    class Resolver {
      constructor(vendor?: string)

      resolve(unit: model.document.BaseUnit): model.document.BaseUnit
      resolve(unit: model.document.BaseUnit, pipeline: string): model.document.BaseUnit
    }
  }

  /* Not exported */
  namespace resource {

    /* Not exported */
    interface ResourceLoader {

      fetch(resource: string): Promise<client.remote.Content>

      accepts(resource: string): boolean
    }
  }

  /* Not exported */
  namespace core {

    export class Vendor {
      static RAML: Vendor
      static RAML08: Vendor
      static RAML10: Vendor
      static OAS: Vendor
      static OAS20: Vendor
      static OAS30: Vendor
      static AMF: Vendor
      static PAYLOAD: Vendor
      static AML: Vendor
    }

    namespace parser {

      class Position {
        constructor(line: number, column: number)

        line: number
        column: number
      }

      class Range {
        constructor(start: core.parser.Position, end: core.parser.Position)

        start: core.parser.Position
        end: core.parser.Position

        toString(): string
      }

    }

    namespace validation {

      export class SeverityLevels {
        static readonly WARNING: 'Warning'
        static readonly INFO: 'Info'
        static readonly VIOLATION: 'Violation'
      }
    }
  }
}